﻿namespace StoryBuilder.Services.Navigation;

public interface INavigable
{
    void Activate(object parameter);
    void Deactivate(object parameter);
}