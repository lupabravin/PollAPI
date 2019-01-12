using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PollAPI.Models.DTO;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using System.Data.Entity.Core;
using System.Web.Http.Results;

namespace PollAPI.Controllers
{
    public class PollController : ApiController
    {
        PollDB PollDBContext = new PollDB();

        #region GET: /poll/:id
        [ResponseType(typeof(poll))]
        [Route("poll/{id}")]
        public IHttpActionResult GetPoll(int id)
        {
            poll poll = PollDBContext.polls.Find(id);

            if (poll == null)
                return NotFound();

            PollDTO Poll = new PollDTO()
            {
                Id = poll.poll_id,
                Description = poll.poll_description,
            };

            var options = JArray.Parse(JsonConvert.SerializeObject(PollDBContext.options.Where(x => x.poll_id == id).Select(x => new { x.option_id, x.option_description }).ToArray()));

            return Json(new { poll_id = poll.poll_id, poll_description = poll.poll_description, options = options });
        }
        #endregion

        #region GET: /poll/:id/stats
        [ResponseType(typeof(poll))]
        [Route("poll/{id}/stats")]
        public IHttpActionResult GetPollStats(int id)
        {
            var poll = PollDBContext.polls.Find(id);

            if (poll == null)
            {
                return NotFound();
            }

            using (var PollContext = new PollDB())
            {
                using (var PollDBContextTransaction = PollDBContext.Database.BeginTransaction())
                {
                    try
                    {
                        var view = new view()
                        {
                            poll_id = id,
                            date = DateTime.Now
                        };

                        PollContext.views.Add(view);
                        PollContext.SaveChanges();
                        PollDBContextTransaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        PollDBContextTransaction.Rollback();
                    }
                }
            }
            var views = PollDBContext.views.Where(x => x.poll_id == id).Count();
            var votes = JArray.Parse(JsonConvert.SerializeObject(from o in PollDBContext.options.Where(option => option.poll_id == id)
                        select new
                        {
                            option_id = o.option_id,
                            qty = PollDBContext.votes.Where(x => x.option_id == o.option_id).Count()
                        }
            ));

            return Json(new { views = views, votes });
        }
        #endregion

        #region POST: /poll
        [ResponseType(typeof(poll))]
        [Route("poll")]

        public IHttpActionResult Post(PollDTO poll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pollModel = new poll()
            {
                poll_description = poll.Description
            };

            using (var PollContext = new PollDB())
            {
                PollContext.Configuration.ProxyCreationEnabled = false;
                using (var PollDBContextTransaction = PollContext.Database.BeginTransaction())
                {
                    try
                    {
                        PollContext.polls.Add(pollModel);
                        PollContext.SaveChanges();
                        poll.Id = pollModel.poll_id;

                        ICollection<option> options = new HashSet<option>();
                        poll.Options.ToList().ForEach(
                            description => options.Add(
                                new option()
                                {
                                    option_description = description,
                                    poll_id = poll.Id,
                                })
                            );

                        PollContext.options.AddRange(options);
                        PollContext.SaveChanges();
                        PollDBContextTransaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        PollDBContextTransaction.Rollback();
                    }
                }
            }

            return Json(new { poll_id = poll.Id });
        }
        #endregion

        #region POST: poll/:id/vote
        [ResponseType(typeof(poll))]
        [Route("poll/{id}/vote")]
        public IHttpActionResult PostVote(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            option votedOption;

            try
            {
                votedOption = PollDBContext.options.Where(x => x.poll_id == id && x.option_id == id).First();
            }
            catch (Exception)
            {
                return NotFound();
            }

            using (var PollContext = new PollDB())
            {
                using (var PollDBContextTransaction = PollDBContext.Database.BeginTransaction())
                {
                    try
                    {
                        var vote = new vote()
                        {
                            option_id = id,
                            date = DateTime.Now
                        };

                        PollContext.votes.Add(vote);
                        PollContext.SaveChanges();
                        PollDBContextTransaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        PollDBContextTransaction.Rollback();
                    }
                }
            }

            return Created("PollApi", "Voted Sucessfully");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                PollDBContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}