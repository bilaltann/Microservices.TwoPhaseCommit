namespace Coordinator.Models
{
    public record Node(string Name)
    {
        public Guid Id { get; set; }

        public ICollection<NodeState> NodeStates { get; set; }
        // burada coordinatorun kordine edeceği bütün servisleri bu entity ile temsil edebilriiz
    }
}
