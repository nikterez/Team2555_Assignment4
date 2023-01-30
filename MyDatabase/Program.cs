﻿using MyDatabase.Models;
using MyDatabase.Data;

namespace MyDatabase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new AppDBContext();
            context.Database.EnsureCreated();

            IList<Candidate> dummyCandidates = new List<Candidate>()
            {

                new Candidate
                {
                    FirstName = "Nikos",
                    MiddleName = "Stamatis",
                    LastName = "Terezakis",
                    BirthDate = new DateTime(1989, 01, 27),
                    Gender = "Male",
                    CountryOfResidence = "Greece",
                    NativeLanguage = "Hellenic",
                    Email = "nikterez@proton.com",
                    LandlineNumber = "210 6713669",
                    MobileNumber = "698 6565706",
                    PhotoIdType = "National ID",
                    PhotoIdNumber = "AB 102938",
                    PhotoIdIssueDate = new DateTime(2005, 10, 08),
                    Addresses = new List<Address>() { new Address { State = "Attiki", City = "Athens", AddressLine = "Nafpliou 22", PostalCode = 15231 } }
                },
                new Candidate
                {
                    FirstName = "Henrik",
                    MiddleName = "",
                    LastName = "Svenskeren",
                    BirthDate = new DateTime(1995, 11, 13),
                    Gender = "Male",
                    CountryOfResidence = "Dernmark",
                    NativeLanguage = "Danish",
                    Email = "sven@hotmail.com",
                    LandlineNumber = "0080 91734628",
                    MobileNumber = "699 136479",
                    PhotoIdType = "Passport",
                    PhotoIdNumber = "HHH - 001 - 829371",
                    PhotoIdIssueDate = new DateTime(2011, 04, 19),
                    Addresses = new List<Address>() { new Address { State = "", City = "Copenghagen", AddressLine = "Olaf Street 22", PostalCode = 40085 } }
                },
                new Candidate
                {
                    FirstName = "Laura",
                    MiddleName = "Isabella",
                    LastName = "Perez",
                    BirthDate = new DateTime(1991, 08, 09),
                    Gender = "Female",
                    CountryOfResidence = "Spain",
                    NativeLanguage = "Spanish",
                    Email = "izzyPerez@yahoo.com",
                    LandlineNumber = "9077 2471658",
                    MobileNumber = "692 0091794",
                    PhotoIdType = "Passport",
                    PhotoIdNumber = "DDD - 002 - 824657",
                    PhotoIdIssueDate = new DateTime(2007, 12, 11),
                    Addresses = new List<Address>()
                    {
                        new Address { State = "", City = "Madrid", AddressLine = "Calle de France 12", PostalCode = 100523 },
                        new Address { State = "", City = "Barcelona", AddressLine = "Calle de Angel 34", PostalCode = 120098 }
                    }
                },
                new Candidate
                {
                    FirstName = "Mary",
                    MiddleName = "",
                    LastName = "Jameson",
                    BirthDate = new DateTime(2000, 11, 19),
                    Gender = "Female",
                    CountryOfResidence = "USA",
                    NativeLanguage = "English",
                    Email = "maryjameson@hotmail.com",
                    LandlineNumber = "(2010) 5874658",
                    MobileNumber = "694 4947621",
                    PhotoIdType = "National ID",
                    PhotoIdNumber = "CBA 552734",
                    PhotoIdIssueDate = new DateTime(2015, 04, 19),
                    Addresses = new List<Address>()
                    {
                        new Address { State = "California", City = "San Diego", AddressLine = "Hoover Avenue 125", PostalCode = 783512 },
                        new Address { State = "Texas", City = "Austin", AddressLine = "Forest Street 56", PostalCode = 501238 }
                    }
                },
                new Candidate
                {
                    FirstName = "Jedrick",
                    MiddleName = "Filip",
                    LastName = "Kowalczyk",
                    BirthDate = new DateTime(1997, 11, 16),
                    Gender = "Male",
                    CountryOfResidence = "Poland",
                    NativeLanguage = "Polish",
                    Email = "kowalczyk97@windows.com",
                    LandlineNumber = "45 7833658",
                    MobileNumber = "695 6471933",
                    PhotoIdType = "National Id",
                    PhotoIdNumber = "XX-01 15978",
                    PhotoIdIssueDate = new DateTime(2015, 12, 08),
                    Addresses = new List<Address>() { new Address { State = "", City = "Warsaw", AddressLine = "Small Street 22", PostalCode = 90078 } }
                },
                new Candidate
                {
                    FirstName = "Claire",
                    MiddleName = "Chloe",
                    LastName = "Dupond",
                    BirthDate = new DateTime(1993, 05, 07),
                    Gender = "Female",
                    CountryOfResidence = "France",
                    NativeLanguage = "French",
                    Email = "cDupond@gmail.com",
                    LandlineNumber = "3001 948415",
                    MobileNumber = "694 7198458",
                    PhotoIdType = "National Id",
                    PhotoIdNumber = "DAS 851245",
                    PhotoIdIssueDate = new DateTime(2010, 01, 09),
                    Addresses = new List<Address>() { new Address { State = "Ile de France", City = "Paris", AddressLine = "Rue Lepic 178", PostalCode = 102385 } }
                }
            };

            context.Candidates.AddRange(dummyCandidates);
            context.SaveChanges();

            IList<Level> levels = new List<Level>()
            {
                new Level {Title="Foundation"},
                new Level {Title="Advanced"},
                new Level {Title="Practitioner"},
                new Level {Title="Expert"}
            };

            context.Levels.AddRange(levels);
            context.SaveChanges();

            IList<Certificate> dummyCertificates = new List<Certificate>()
            {
                new Certificate
                {
                    Title="Software Development",
                    Description="SD Desc A", Status="Available",
                    Level= context.Levels.Find(1),

                },
                new Certificate
                {
                    Title="Software Development",
                    Description="SD Desc B", Status="Unavailable",
                    Level= context.Levels.Find(2),

                },
                new Certificate
                {
                    Title="Dev Ops",
                    Description="DO Desc A", Status="Available",
                    Level= context.Levels.Find(3),

                },
                new Certificate
                {
                    Title="Dev Ops",
                    Description="DO Desc B", Status="Available",
                    Level= context.Levels.Find(4),

                },
                new Certificate
                {
                    Title="Agile",
                    Description="A Desc A", Status="Available",
                    Level= context.Levels.Find(1),

                },
                new Certificate
                {
                    Title="Scrum",
                    Description="S Desc A", Status="Available",
                    Level= context.Levels.Find(3),

                },
                new Certificate
                {
                    Title="Scrum",
                    Description="S Desc B", Status="Unavailable",
                    Level= context.Levels.Find(4),

                },
            };

            context.Certificates.AddRange(dummyCertificates);
            context.SaveChanges();

            IList<QuestionDifficulty> difficulties = new List<QuestionDifficulty>()
            {
                new QuestionDifficulty {Difficulty="Easy"},
                new QuestionDifficulty {Difficulty="Intermediate"},
                new QuestionDifficulty {Difficulty="Advanced"}
            };

            context.QuestionDifficulties.AddRange(difficulties);
            context.SaveChanges();

            IList<Question> dummyQuestions = new List<Question>()
            {
                new Question {Display="Question1", QuestionDifficulty=context.QuestionDifficulties.Find(1) },
                new Question {Display="Question2", QuestionDifficulty=context.QuestionDifficulties.Find(3) },
                new Question {Display="Question3", QuestionDifficulty=context.QuestionDifficulties.Find(1) },
                new Question {Display="Question4", QuestionDifficulty=context.QuestionDifficulties.Find(2) },
                new Question {Display="Question5", QuestionDifficulty=context.QuestionDifficulties.Find(1) },
                new Question {Display="Question6", QuestionDifficulty=context.QuestionDifficulties.Find(2) },
                new Question {Display="Question7", QuestionDifficulty=context.QuestionDifficulties.Find(2) },
                new Question {Display="Question8", QuestionDifficulty=context.QuestionDifficulties.Find(1) },
                new Question {Display="Question9", QuestionDifficulty=context.QuestionDifficulties.Find(2) },
                new Question {Display="Question10", QuestionDifficulty=context.QuestionDifficulties.Find(3) }
            };

            context.Questions.AddRange(dummyQuestions);
            context.SaveChanges();

            IList<QuestionPossibleAnswer> questionPossibleAnswers = new List<QuestionPossibleAnswer>()
            {
                new QuestionPossibleAnswer {Question=context.Questions.Find(1) ,PossibleAnswer="Answer1", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(1) ,PossibleAnswer="Answer2", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(1) ,PossibleAnswer="Answer3", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(1) ,PossibleAnswer="Answer4", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(2) ,PossibleAnswer="Answer1", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(2) ,PossibleAnswer="Answer2", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(2) ,PossibleAnswer="Answer3", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(2) ,PossibleAnswer="Answer4", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(3) ,PossibleAnswer="Answer1", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(3) ,PossibleAnswer="Answer2", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(3) ,PossibleAnswer="Answer3", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(3) ,PossibleAnswer="Answer4", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(4) ,PossibleAnswer="Answer1", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(4) ,PossibleAnswer="Answer2", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(4) ,PossibleAnswer="Answer3", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(4) ,PossibleAnswer="Answer4", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(5) ,PossibleAnswer="Answer1", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(5) ,PossibleAnswer="Answer2", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(5) ,PossibleAnswer="Answer3", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(5) ,PossibleAnswer="Answer4", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(6) ,PossibleAnswer="Answer1", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(6) ,PossibleAnswer="Answer2", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(6) ,PossibleAnswer="Answer3", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(6) ,PossibleAnswer="Answer4", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(7) ,PossibleAnswer="Answer1", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(7) ,PossibleAnswer="Answer2", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(7) ,PossibleAnswer="Answer3", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(7) ,PossibleAnswer="Answer4", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(8) ,PossibleAnswer="Answer1", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(8) ,PossibleAnswer="Answer2", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(8) ,PossibleAnswer="Answer3", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(8) ,PossibleAnswer="Answer4", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(9) ,PossibleAnswer="Answer1", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(9) ,PossibleAnswer="Answer2", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(9) ,PossibleAnswer="Answer3", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(9) ,PossibleAnswer="Answer4", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(10) ,PossibleAnswer="Answer1", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(10) ,PossibleAnswer="Answer2", IsCorrect=false},
                new QuestionPossibleAnswer {Question=context.Questions.Find(10) ,PossibleAnswer="Answer3", IsCorrect=true},
                new QuestionPossibleAnswer {Question=context.Questions.Find(10) ,PossibleAnswer="Answer4", IsCorrect=false}
            };

            context.QuestionPossibleAnswers.AddRange(questionPossibleAnswers);
            context.SaveChanges();

            IList<Topic> dummyTopics = new List<Topic>()
            {
                new Topic {Title="Topic T1", Description="T1 Desc"},
                new Topic {Title="Topic T2", Description="T2 Desc"},
                new Topic {Title="Topic T3", Description="T3 Desc"},
                new Topic {Title="Topic T4", Description="T4 Desc"},
                new Topic {Title="Topic T5", Description="T5 Desc"},
                new Topic {Title="Topic T6", Description="T6 Desc"},
                new Topic {Title="Topic T7", Description="T7 Desc"},
                new Topic {Title="Topic T8", Description="T8 Desc"},

            };

            context.Topics.AddRange(dummyTopics);
            context.SaveChanges();

            IList<CertificateTopic> certificateTopics = new List<CertificateTopic>()
            {
                new CertificateTopic//1
                {
                    Certificate=context.Certificates.Find(1),
                    Topic=context.Topics.Find(1)
                },
                new CertificateTopic//2
                {
                    Certificate=context.Certificates.Find(1),
                    Topic=context.Topics.Find(2)
                },
                new CertificateTopic//3
                {
                    Certificate=context.Certificates.Find(1),
                    Topic=context.Topics.Find(3)
                },
                new CertificateTopic//4
                {
                    Certificate=context.Certificates.Find(1),
                    Topic=null
                },
                new CertificateTopic//5
                {
                    Certificate=context.Certificates.Find(3),
                    Topic=context.Topics.Find(4)
                },
                new CertificateTopic//6
                {
                    Certificate=context.Certificates.Find(3),
                    Topic=context.Topics.Find(5)
                },
                new CertificateTopic//7
                {
                    Certificate=context.Certificates.Find(3),
                    Topic=context.Topics.Find(6)
                }
            };

            context.CertificateTopics.AddRange(certificateTopics);
            context.SaveChanges();

            IList<TopicQuestion> topicQuestions = new List<TopicQuestion>()
            {
                new TopicQuestion
                {
                    Topic= context.Topics.Find(1),
                    Question= context.Questions.Find(1)
                },
                new TopicQuestion
                {
                    Topic= context.Topics.Find(1),
                    Question= context.Questions.Find(2)
                },
                new TopicQuestion
                {
                    Topic= context.Topics.Find(2),
                    Question= context.Questions.Find(3)
                },
                new TopicQuestion
                {
                    Topic= context.Topics.Find(3),
                    Question= context.Questions.Find(4)
                },
                new TopicQuestion //5
                {
                    Topic= context.Topics.Find(4),
                    Question= context.Questions.Find(5)
                },
                new TopicQuestion //6
                {
                    Topic= context.Topics.Find(4),
                    Question= context.Questions.Find(6)
                },
                new TopicQuestion //7
                {
                    Topic= context.Topics.Find(5),
                    Question= context.Questions.Find(7)
                },
                new TopicQuestion//8
                {
                    Topic= context.Topics.Find(6),
                    Question=context.Questions.Find(8)
                },
                new TopicQuestion
                {
                    Topic= null,
                    Question=context.Questions.Find(9)
                }
            };

            context.TopicQuestions.AddRange(topicQuestions);
            context.SaveChanges();

            IList<CertificateTopicQuestion> certificateTopicQuestions = new List<CertificateTopicQuestion>()
            {
                new CertificateTopicQuestion
                {
                    CertificateTopic = context.CertificateTopics.Find(1),
                    TopicQuestion= context.TopicQuestions.Find(1)
                },
                new CertificateTopicQuestion
                {
                    CertificateTopic= context.CertificateTopics.Find(1),
                    TopicQuestion= context.TopicQuestions.Find(2)
                },
                new CertificateTopicQuestion
                {
                    CertificateTopic = context.CertificateTopics.Find(2),
                    TopicQuestion= context.TopicQuestions.Find(3)
                },
                new CertificateTopicQuestion
                {
                    CertificateTopic = context.CertificateTopics.Find(3),
                    TopicQuestion= context.TopicQuestions.Find(4)
                },
                new CertificateTopicQuestion
                {
                    CertificateTopic= context.CertificateTopics.Find(4),
                    TopicQuestion= context.TopicQuestions.Find(9)
                },
                new CertificateTopicQuestion
                {
                    CertificateTopic = context.CertificateTopics.Find(5),
                    TopicQuestion= context.TopicQuestions.Find(5)
                },
                new CertificateTopicQuestion
                {
                    CertificateTopic = context.CertificateTopics.Find(5),
                    TopicQuestion= context.TopicQuestions.Find(6)
                },
                new CertificateTopicQuestion
                {
                    CertificateTopic = context.CertificateTopics.Find(6),
                    TopicQuestion= context.TopicQuestions.Find(7)
                },
                new CertificateTopicQuestion
                {
                    CertificateTopic = context.CertificateTopics.Find(7),
                    TopicQuestion= context.TopicQuestions.Find(8)
                }
            };

            context.CertificateTopicQuestions.AddRange(certificateTopicQuestions);
            context.SaveChanges();

            IList<Examination> examinations = new List<Examination>()
            {
                new Examination {Certificate=context.Certificates.Find(1)},
                new Examination {Certificate=context.Certificates.Find(1)},
                new Examination {Certificate=context.Certificates.Find(3)},
                new Examination {Certificate=context.Certificates.Find(3)}

            };

            context.Examinations.AddRange(examinations);
            context.SaveChanges();

            IList<CandidateExam> candidateExams = new List<CandidateExam>
            {
                new CandidateExam
                {
                    ExamCode="A001",
                    ExamDate=new DateTime(2001-1-1),
                    ResultIssueDate=new DateTime(2001-1-2),
                    CandidateTotalScore=100,
                    Candidate=context.Candidates.Find(2),
                    Examination=context.Examinations.Find(1),
                    ResultLabel="Pass"
                }
            };

            context.CandidateExams.AddRange(candidateExams);
            context.SaveChanges();

            IList<ExaminationQuestion> examQuestions = new List<ExaminationQuestion>()
            {
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(1),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(1)
                },
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(1),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(2)
                },
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(1),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(3)
                },
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(2),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(3)
                },
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(2),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(4)
                },
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(2),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(5)
                },
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(3),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(6)
                },
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(3),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(7)
                },
                new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(4),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(8)
                },new ExaminationQuestion
                {
                    Examination =context.Examinations.Find(4),
                    CertificateTopicQuestion =context.CertificateTopicQuestions.Find(9)
                }
            };

            context.ExamQuestions.AddRange(examQuestions);
            context.SaveChanges();

            IList<ExamCandidateAnswer> examCandidateAnswers = new List<ExamCandidateAnswer>()
            {
                new ExamCandidateAnswer
                {
                    SelectedAnswer="1",
                    CorrectAnswer="1",
                    CandidateExam=context.CandidateExams.Find(1),
                    CertificateTopicQuestion=context.CertificateTopicQuestions.Find(1)
                },
                new ExamCandidateAnswer
                {
                    SelectedAnswer="2",
                    CorrectAnswer="4",
                    CandidateExam=context.CandidateExams.Find(1),
                    CertificateTopicQuestion=context.CertificateTopicQuestions.Find(2)
                },
                new ExamCandidateAnswer
                {
                    SelectedAnswer="2",
                    CorrectAnswer="2",
                    CandidateExam=context.CandidateExams.Find(1),
                    CertificateTopicQuestion=context.CertificateTopicQuestions.Find(3)
                }
                
            };

            context.ExamCandidateAnswers.AddRange(examCandidateAnswers);
            context.SaveChanges();

        }
    }
}
