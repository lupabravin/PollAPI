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
        [ResponseType(typeof(Poll))]
        [Route("poll/{id}")]
        public IHttpActionResult GetPoll(int id)
        {
            Poll poll = PollDBContext.Polls.Find(id);

            if (poll == null)
                return NotFound();

            PollDTO Poll = new PollDTO()
            {
                Id = poll.poll_id,
                Description = poll.poll_description,
            };

            var options = JArray.Parse(JsonConvert.SerializeObject(PollDBContext.Options.Where(x => x.poll_id == id).Select(x => new { x.option_id, x.option_description }).ToArray()));

            return Json(new { poll_id = poll.poll_id, poll_description = poll.poll_description, options = options });
        }
        #endregion

        #region GET: /poll/:id/stats
        [ResponseType(typeof(Poll))]
        [Route("poll/{id}/stats")]
        public IHttpActionResult GetPollStats(int id)
        {
            var poll = PollDBContext.Polls.Find(id);

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
                        var view = new View()
                        {
                            poll_id = id,
                            date = DateTime.Now
                        };

                        PollContext.Views.Add(view);
                        PollContext.SaveChanges();
                        PollDBContextTransaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        PollDBContextTransaction.Rollback();
                    }
                }
            }
            var views = PollDBContext.Views.Where(x => x.poll_id == id).Count();
            var votes = JArray.Parse(JsonConvert.SerializeObject(from o in PollDBContext.Options.Where(option => option.poll_id == id)
                        select new
                        {
                            option_id = o.option_id,
                            qty = PollDBContext.Votes.Where(x => x.option_id == o.option_id).Count()
                        }
            ));

            return Json(new { views = views, votes });
        }
        #endregion

        #region POST: /poll
        [ResponseType(typeof(Poll))]
        [Route("poll")]

        public IHttpActionResult Post(PollDTO poll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pollModel = new Poll()
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
                        PollContext.Polls.Add(pollModel);
                        PollContext.SaveChanges();
                        poll.Id = pollModel.poll_id;

                        ICollection<Option> options = new HashSet<Option>();
                        poll.Options.ToList().ForEach(
                            description => options.Add(
                                new Option()
                                {
                                    option_description = description,
                                    poll_id = poll.Id
                                })
                            );

                        PollContext.Options.AddRange(options);
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
        [ResponseType(typeof(Poll))]
        [Route("poll/{id}/vote")]
        public IHttpActionResult PostVote(OptionDTO option, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Option votedOption = option;

            try
            {
                votedOption = PollDBContext.Options.Where(x => x.poll_id == id && x.option_id == option.option_id).First();
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
                        var vote = new Vote()
                        {
                            option_id = votedOption.option_id,
                            date = DateTime.Now
                        };

                        PollContext.Votes.Add(vote);
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