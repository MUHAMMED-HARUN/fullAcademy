
using System.Domain.Events.Base;

namespace System.Domain.Entities.BaseClass
{
    public abstract class clsBaseClass
    {
        public Guid Id { get; private set; }
        public string CreatedBy { get; private set; }
        public DateOnly CreatedAt { get; private set; }
        public string? ModifiedBy { get; private set; }
        public DateOnly? ModifiedAt { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateOnly? DeletedAt { get; private set; }
        public bool IsActive { get; private set; }

        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected clsBaseClass() { } // For EF

        public void SetCreated(string userId)
        {
            BaseSetCreated(userId);
        }

        protected void BaseSetCreated(string userId)
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
            CreatedBy = userId;
            CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            IsActive = true;
        }

        public virtual void SetModified(string userId)
        {
            ModifiedBy = userId;
            ModifiedAt = DateOnly.FromDateTime(DateTime.Now);
        }

        public virtual void SetDeleted(string userId)
        {
            DeletedBy = userId;
            DeletedAt = DateOnly.FromDateTime(DateTime.Now);
            IsActive = false; // Soft delete usually implies deactivating
        }

        public virtual void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        // Helper method to set ID if needed explicitly, though usually handled in creation
        public void SetID(Guid id)
        {
            Id = id;
        }
    }
}
