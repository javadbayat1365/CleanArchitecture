﻿namespace Application.Contracts.FileService.Models;

public record SaveFileModel(string Base64File, string FileContent); //FileContent : image
