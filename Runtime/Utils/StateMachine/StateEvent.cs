namespace Potency.Services.Runtime.Utils.StateMachine
{
    public class StateEvent : IStateEvent
    {
        private static int _id;
        
        public int Id { get; }
        public string Name { get; }
        
        public StateEvent(string name)
        {
            Id = ++_id;
            Name = name;
        }

        public bool Equals(IStateEvent other)
        {
            return other != null && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
        
        public override string ToString()
		{
			return Name;
		}
    }
}