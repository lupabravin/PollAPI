using PollAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PollAPI.Services
{
    public class PollService
    {
        public PollService()
        {
        }

        public void AddViewToPoll(int id)
        {
            using (var PollContext = new PollDB())
            {
                using (var PollDBContextTransaction = PollContext.Database.BeginTransaction())
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
        }

        public void AddVoteToPoll(int option_id)
        {
            using (var PollContext = new PollDB())
            {
                using (var PollDBContextTransaction = PollContext.Database.BeginTransaction())
                {
                    try
                    {
                        var vote = new Vote()
                        {
                            option_id = option_id,
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
        }

        public void CreatePoll(PollDTO poll)
        {
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
        }
    }
}