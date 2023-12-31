﻿using Domain.Primitives;

namespace Domain.Customers;

public class Customer : Entity<CustomerId>
{
    public Customer(CustomerId id, string email, string name, string identityId)
    {
        Id = id;
        Email = email;
        Name = name;
        IdentityId = identityId;
    }

    public CustomerId Id { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string IdentityId { get; private set; }
}
