namespace HardPedia.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Role { get; set; }
        public UserDTO() { }
        public UserDTO(string id, string userName, string email, string phoneNumber, string role)
        {
            Id = id;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Role = role;
        }

    }
}
