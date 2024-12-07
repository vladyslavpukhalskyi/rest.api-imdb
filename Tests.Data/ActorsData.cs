using Domain.Actors;
using System;

namespace Tests.Data;

public static class ActorsData
{
    public static Actor MainActor => Actor.New(
        ActorId.New(),
        "TestFirstName",
        "TestLastName",
        new DateTime(1985, 1, 1));
    
    public static Actor SecondaryActor => Actor.New(
        ActorId.New(),
        "SecondaryFirstName",
        "SecondaryLastName",
        new DateTime(1990, 6, 15));
}