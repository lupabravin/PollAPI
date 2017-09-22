using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PollAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PollAPI.Controllers
{
    public class PollsController : ApiController
    {
        private PollDBModel db = new PollDBModel();

        #region GET /poll
        // GET: api/poll
        [Route("poll")]
        public IQueryable<Poll> Getpoll()
        {
            return db.Poll;
        }
        #endregion

        #region GET: /poll/:id
        [ResponseType(typeof(Poll))]
        [Route("poll/{id}")]
        public IHttpActionResult GetPoll(int id)
        {
            Poll poll = db.Poll.Find(id);
            if (poll == null)
            {
                return NotFound();
            }

            var options = JArray.Parse(JsonConvert.SerializeObject(db.Option.Where(x => x.poll_id == id).Select(x => new { x.option_id, x.option_description }).ToArray()));

            return Json(new { poll_id = poll.poll_id, poll_description = poll.poll_description, options = options });
        }
        #endregion

        #region GET: /poll/:id/stats
        [ResponseType(typeof(Poll))]
        [Route("poll/{id}/stats")]
        public IHttpActionResult GetPollStats(int id)
        {
            Poll poll = db.Poll.Find(id);
            if (poll == null)
            {
                return NotFound();
            }

            poll.views++;

            var votes = JArray.Parse(JsonConvert.SerializeObject(db.Option.Where(x => x.poll_id == id).Select(x => new { x.option_id, x.qty }).ToArray()));
            db.SaveChanges();

            return Json(new { views = poll.views, votes});
        }
        #endregion

        #region POST: /poll
        [ResponseType(typeof(Poll))]
        [Route("poll")]

        public IHttpActionResult PostPoll(Poll poll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Poll.Add(poll);
            db.SaveChanges();
            var options = new HashSet<Option>();
            int id = 0;
            poll.options.ToList().ForEach(
                description => options.Add(
                    new Option
                    {
                        option_id = ++id,
                        poll_id = poll.poll_id,
                        option_description = description
                    }
                )
            );

            db.Option.AddRange(options);
            db.SaveChanges();

            return Json(new
            {
                poll_id = poll.poll_id
            });

        }
        #endregion

        #region POST: poll/:id/vote
        [ResponseType(typeof(Poll))]
        [Route("poll/{id}/vote")]
        public IHttpActionResult Postvote(Option option, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Option votedOption;

            try
            {
                votedOption = db.Option.Where(x => x.poll_id == id && x.option_id == option.option_id).First();
            }
            catch (Exception)
            {
                return NotFound();
            }

            votedOption.qty++;

            db.SaveChanges();

            return Created("PollApi", "Voted Sucessfully");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PollExists(int id)
        {
            return db.Poll.Count(e => e.poll_id == id) > 0;
        }
    }
}