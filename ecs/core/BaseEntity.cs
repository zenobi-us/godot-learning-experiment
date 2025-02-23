// EntityManager.cs
namespace core
{
    // Entity class - just an ID container
    public class BaseEntity
    {
        public int Id { get; private set; }

        public BaseEntity(int id)
        {
            Id = id;
        }
    }

}
