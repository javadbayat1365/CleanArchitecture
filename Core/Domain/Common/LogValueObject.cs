﻿namespace Domain.Common;

public record LogValueObject(DateTime RegisterDate,string Message,string? AdditionalMessage = null);
