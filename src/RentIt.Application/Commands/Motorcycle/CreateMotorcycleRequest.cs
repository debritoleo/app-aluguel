﻿namespace RentIt.Application.Commands.Motorcycle;

public class CreateMotorcycleRequest
{
    public string Identificador { get; set; } = string.Empty;

    public int Ano { get; set; }

    public string Modelo { get; set; } = string.Empty;

    public string Placa { get; set; } = string.Empty;
}

