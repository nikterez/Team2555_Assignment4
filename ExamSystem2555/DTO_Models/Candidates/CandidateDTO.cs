using MyDatabase.Models;

namespace WebApp.DTO_Models.Candidates
{
    public class CandidateDTO
    {
        public int CandidateId { get; set; }
        public string? UserCandidateId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string CountryOfResidence { get; set; }
        public string NativeLanguage { get; set; }
        public string Email { get; set; }
        public string? LandlineNumber { get; set; }
        public string MobileNumber { get; set; }
        public string PhotoIdType { get; set; }
        public string PhotoIdNumber { get; set; }
        public DateTime PhotoIdIssueDate { get; set; }
    }
}