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
using PollAPI.Services;

namespace PollAPI.Controllers
{
    public class PollController : ApiController
    {
        PollDB PollDBContext = new PollDB();
        PollService PollService = new PollService();
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

            PollService.AddViewToPoll(id);

            var options = JArray.Parse(JsonConvert.SerializeObject(PollDBContext.Options.Where(o => o.poll_id == id).Select(o => new { o.option_id, o.option_description }).ToArray()));

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

            var views = PollDBContext.Views.Where(v => v.poll_id == id).Count();
            var votes = JArray.Parse(JsonConvert.SerializeObject(
                from o in PollDBContext.Options.Where(option => option.poll_id == id)
                select new
                {
                    option_id = o.option_id,
                    qty = PollDBContext.Votes.Where(v => v.option_id == o.option_id).Count()
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

            PollService.CreatePoll(poll);
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
                votedOption = PollDBContext.Options.Where(o => o.poll_id == id && o.option_id == option.option_id).First();
            }
            catch (Exception)
            {
                return NotFound();
            }

            PollService.AddVoteToPoll(votedOption.option_id);

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