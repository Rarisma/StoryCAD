using Windows.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using StoryCAD.DAL;
using StoryCAD.Models;
using StoryCAD.Models.Tools;
using StoryCAD.Services.Backend;
using System;

namespace StoryCAD.ViewModels.Tools;

/// <summary>
/// This is the PreferencesInitialization ViewModel, this data
/// </summary>
public class InitVM : ObservableRecipient
{

    /// <summary>
    /// This is the constructor for InitVM.
    /// It sets the paths for Path and Backup Path to
    /// \UserFolder\Documents\StoryCAD\ and then Projects or backups respectively.
    ///
    /// For example this would give the following path for me
    /// C:\Users\Jake\Documents\StoryCAD\Projects
    /// </summary>
    public InitVM()
    {
        Preferences.ProjectDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StoryCAD", "Projects");
        Preferences.BackupDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StoryCAD", "Backups");
    }

    public PreferencesModel Preferences = new();

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    /// <summary>
    /// This saves and checks the input of the user.
    /// It creates a new preferences model and stores the
    /// inputs of the user such as path, name, email ect.
    ///
    /// This will then be saved and the backup path and project
    /// path folders will be created.
    /// </summary>
    public async void Save()
    {
        //Create paths
        System.IO.Directory.CreateDirectory(Preferences.BackupDirectory);
        System.IO.Directory.CreateDirectory(Preferences.ProjectDirectory);

        Preferences.PreferencesInitialized = true; //Make sure prefs init page isn't shown again

        //Updates the file, then rereads into memory.
        PreferencesIo _prfIo = new(Preferences, System.IO.Path.Combine(ApplicationData.Current.RoamingFolder.Path,"StoryCAD"));
        await _prfIo.WritePreferences ();
        PreferencesIo _loader = new(GlobalData.Preferences, System.IO.Path.Combine(ApplicationData.Current.RoamingFolder.Path, "StoryCAD"));
        await _loader.ReadPreferences();
        BackendService _backend = Ioc.Default.GetRequiredService<BackendService>();
        if (!GlobalData.Preferences.RecordPreferencesStatus) { await _backend.PostPreferences(GlobalData.Preferences); }
    }
}