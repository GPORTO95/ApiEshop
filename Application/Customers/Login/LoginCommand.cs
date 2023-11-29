﻿using Application.Abstractions.Messaging;

namespace Application.Customers.Login;

public record LoginCommand(string Email, string Passoword) : ICommand<string>;
