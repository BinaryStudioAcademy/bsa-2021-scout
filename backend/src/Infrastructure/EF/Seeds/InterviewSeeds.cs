using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class InterviewSeeds
    {
        private static Random _random = new Random();

        private static IList<string> ApplicantsIds = ApplicantSeeds.GetApplicants().Select(x=>x.Id).ToList();


        private static string getLinkStart(MeetingSource source){
            switch(source)
            {
                case MeetingSource.GoogleMeet:
                    return "https://meet.google.com/";
                case MeetingSource.Zoom:
                    return "https://us.zoom.com/";
                case MeetingSource.Slack:
                    return "https://slack.com/";
                case MeetingSource.Skype:
                    return "https://skype.com/";
                case MeetingSource.MSTeams:
                    return "https://ms-teams.com/";
                default:
                    return "";
            }
        }
        private static Interview GenerateInterview(string id)
        {
            MeetingSource source = sources[_random.Next(sources.Count())];
            string link = getLinkStart(source) + linksEndings[_random.Next(linksEndings.Count())];
            int randomIndex = _random.Next(titles.Count());
            DateTime created = Common.GetRandomDateTime(new DateTime(2021, 07, 01));
            DateTime schedualed = created.AddDays(_random.Next(130))
            .AddHours(_random.Next(7, 19))
            .AddMinutes(_random.Next(15, 45));
            
            return new Interview
            {
                Id = id,
                Title = titles[randomIndex],
                MeetingSource = source,
                MeetingLink = link,
                CreatedDate = created,
                InterviewType = types[_random.Next(types.Count())],
                CandidateId = ApplicantsIds[_random.Next(ApplicantsIds.Count())],
                Scheduled = schedualed,
                Note = notes[_random.Next(notes.Count())],
                Duration = _random.Next(45, 90),
                VacancyId = VacancySeeds.vacancyIds[_random.Next(VacancySeeds.vacancyIds.Count())],
                CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                IsReviewed = true
            };
        }
        public static IEnumerable<Interview> GetInterviews()
        {
            List<Interview> list = new List<Interview>();

            foreach (string id in interviewIds)
            {
                list.Add(GenerateInterview(id));
            }

            return list;
        }

        private static IList<MeetingSource> sources = new List<MeetingSource> {
            MeetingSource.GoogleMeet,
            MeetingSource.MSTeams,
            MeetingSource.Skype,
            MeetingSource.Slack,
            MeetingSource.Zoom
        };

        private static IList<InterviewType> types = new List<InterviewType> {
           InterviewType.Behavioural,
           InterviewType.Interview,
           InterviewType.Meeting,
           InterviewType.OnSite,
           InterviewType.Technical
        };

        private static List<string> linksEndings = new List<string> {
            "zxn-ayjm-aar",
            "ndf-haoc-udu",
            "nde-eyww-erj",
            "cvs-ddur-hju",
            "naj-kcct-aji",
            "nwe-eyrw-eej",
            "cqs-ddwr-heu",
            "qaj-kcet-aqi",
        };

        private static List<string> notes = new List<string> {
            "Check English level",
            "Ask about dog tolerance.",
            "Ask about cat tolerance.",
            "Check German language knowledge",
            "",
        };
        private static IList<string> titles = new List<string> {
            "Devops",
            "QA",
            "Software Engineer",
            "Project Designer",
            "UI/UX Frontend",
            "Developer",
            "Web Developer",
            "Project Manager"
        };

        public static List<string> interviewIds = new List<string> {
            "e2615472-217b-4bf9-a481-6b9420fa8c5a",
            "960f1cf4-d0b5-44e5-a993-47f972d1ff14",
            "94491d9c-e538-446f-b355-ee2d5887a757",
            "c78fbcd5-0d28-4210-9079-e159c15a5925",
            "c9160ec7-25b9-4f05-ba1d-d63eac2e894c",
            "e6a10c7c-02b0-44e2-81d1-55a4e44b30f8",
            "879807e4-cc6a-49bf-a07f-41a77a57e5ce",
            "7b922f68-0302-4343-a181-58390759330a",
            "1ccfc868-0c4a-45dd-b290-9f8b7712568c",
            "c4adeddd-ddd0-4c35-82af-547a048e5de0",
            "8b90b81e-76a7-4d2b-b593-31e5a4083c2b",
            "78037dfd-969f-4e60-96e7-3cbfd94ca943",
            "a970aa2b-4cb7-4e4a-9e15-2748fe8b4103",
            "5d0a8836-823a-4250-89fc-6c32746b1ca9",
            "ed43d006-39cc-4c9e-ba0c-3105754371b5",
            "723df3f6-d23b-4e4a-aeda-e042328e8624",
            "2e17d790-93fe-4b2d-8ac4-24825b6fabd9",
            "c8bbb30d-e8e4-4c2e-b6e1-9431becfbfd4",
            "86af017c-a216-44e9-8ac6-bbca3bc2a608",
            "cad43ab5-8045-46cf-8b85-d242762cc1db",
            "16a3e5f4-d270-48d9-9ede-67f787f00883",
            "60ca17a6-2c4b-4094-a8fb-aa99529e48c4",
            "00c5b7ec-a487-4cfc-822d-f1f3178403b4",
            "bfa39239-8d51-4076-a96e-49b908327a0c",
            "b304576e-c65c-420b-884f-12c13faa9c31",
            "dfcbc58c-af1d-401f-b2bb-713ba0651af2",
            "7c234050-2e8e-4fd0-a480-04137ef0903a",
            "f08e0b44-66f0-4102-86a5-da0f611ef2ed",
            "ac6ef3f5-1ce3-41b4-877d-71745a158d20",
            "19760c65-c606-4331-adbd-51dc301af895",
        };
    }
}