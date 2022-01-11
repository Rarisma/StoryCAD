﻿using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using NRtfTree.Util;
using StoryBuilder.DAL;
using StoryBuilder.Models;
using StoryBuilder.ViewModels;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using StoryBuilder.Services.Logging;

namespace StoryBuilder.Services.Reports;

public class ReportFormatter
{
    private Dictionary<string, string[]> _templates = new();
    private StoryReader _rdr;
    private StoryModel _model;

    #region Public methods

    public string FormatStoryOverviewReport(StoryElement element)
    {
        OverviewModel overview = (OverviewModel)element;
        string[] lines = _templates["Story Overview"];  
        RtfDocument doc = new(string.Empty);

        StoryElement vpChar = StringToStoryElement(overview.ViewpointCharacter);
        string vpName = vpChar?.Name ?? string.Empty;
        StoryElement seProblem = StringToStoryElement(overview.StoryProblem);
        string problemName = seProblem?.Name ?? string.Empty;
        ProblemModel problem = (ProblemModel)seProblem;
        string premise = problem?.Premise ?? string.Empty;

        // Parse and write the report
        foreach (string line in lines)
        {
            // Parse the report
            StringBuilder sb = new(line);
            sb.Replace("@Title", overview.Name);
            sb.Replace("@CreateDate", overview.DateCreated);
            sb.Replace("@ModifiedDate", overview.DateModified);
            sb.Replace("@Author", overview.Author);
            sb.Replace("@StoryType", overview.StoryType);
            sb.Replace("@Genre", overview.StoryGenre);
            sb.Replace("@Viewpoint", overview.Viewpoint);
            sb.Replace("@StoryIdea", GetText(overview.StoryIdea));
            sb.Replace("@Concept", GetText(overview.Concept));
            sb.Replace("@StoryProblem", problemName);
            sb.Replace("@Premise", GetText(premise));
            sb.Replace("@StoryType", overview.StoryType);
            sb.Replace("@StoryGenre", overview.StoryGenre);
            sb.Replace("@LiteraryDevice", overview.LiteraryDevice);
            sb.Replace("@viewpointCharacter", vpName);
            sb.Replace("@Voice", overview.Voice);
            sb.Replace("@Tense", overview.Tense);
            sb.Replace("@Style", overview.Style);
            sb.Replace("@StructureNotes", GetText(overview.StructureNotes));
            sb.Replace("@Tone", overview.Tone);
            sb.Replace("@toneNotes", GetText(overview.ToneNotes));
            sb.Replace("@Notes", GetText(overview.Notes));
            doc.AddText(sb.ToString());
            doc.AddNewLine();
        }
        return doc.GetRtf();
    }

    public string FormatProblemListReport()
    {
        string[] lines = _templates["List of Problems"];
        RtfDocument doc = new(string.Empty);

        // Parse and write the report
        foreach (string line in lines)
        {
            if (line.Contains("@Description"))
            {
                foreach (StoryElement element in _model.StoryElements)
                    if (element.Type == StoryItemType.Problem)
                    {
                        ProblemModel chr = (ProblemModel)element;
                        StringBuilder sb = new(line);
                        sb.Replace("@Description", chr.Name);
                        doc.AddText(sb.ToString());
                        doc.AddNewLine();
                    }
            }
            else
            {
                doc.AddText(line);
                doc.AddNewLine();
            }
        }
        return doc.GetRtf();
    }

    public string FormatProblemReport(StoryElement element)
    {
        ProblemModel problem = (ProblemModel)element;
        string[] lines = _templates["Problem Description"];
        RtfDocument doc = new(string.Empty);

        StoryElement vpProtagnoist = StringToStoryElement(problem.Protagonist);

        StoryElement vpAntagonist = StringToStoryElement(problem.Antagonist);

        // Parse and write the report
        foreach (string line in lines)
        {
            StringBuilder sb = new(line);
            sb.Replace("@Title", problem.Name);
            sb.Replace("@ProblemType", problem.ProblemType);
            sb.Replace("@ConflictType", problem.ConflictType);
            sb.Replace("@Subject", problem.Subject);
            sb.Replace("@StoryQuestion", GetText(problem.StoryQuestion));
            sb.Replace("@ProblemSource", problem.ProblemSource);
            sb.Replace("@ProtagName", vpProtagnoist.Name);
            sb.Replace("@ProtagMotive", problem.ProtMotive);
            sb.Replace("@ProtagGoal", problem.ProtGoal);
            sb.Replace("@AntagName", vpAntagonist.Name);
            sb.Replace("@AntagMotive", problem.AntagMotive);
            sb.Replace("@AntagGoal", problem.AntagGoal);
            sb.Replace("@Outcome", problem.Outcome);
            sb.Replace("@Method", problem.Method);
            sb.Replace("@Theme", problem.Theme);
            sb.Replace("@Premise", GetText(problem.Premise));
            sb.Replace("@Notes", GetText(problem.Notes));
            doc.AddText(sb.ToString());
            doc.AddNewLine();
        }

        return doc.GetRtf();
    }

    public string FormatCharacterListReport()
    {
        string[] lines = _templates["List of Characters"];
        RtfDocument doc = new(string.Empty);

        // Parse and write the report
        foreach (string line in lines)
        {
            if (line.Contains("@Description"))
            {
                foreach (StoryElement element in _model.StoryElements)
                    if (element.Type == StoryItemType.Character)
                    {
                        CharacterModel chr = (CharacterModel)element;
                        StringBuilder sb = new(line);
                        sb.Replace("@Description", chr.Name);
                        doc.AddText(sb.ToString());
                        doc.AddNewLine();
                    }
            }
            else
            {
                doc.AddText(line);
                doc.AddNewLine();
            }
        }
        return doc.GetRtf();
    }

    public string FormatCharacterReport(StoryElement element)
    {
        CharacterModel character = (CharacterModel)element;
        string[] lines = _templates["Character Description"];
        RtfDocument doc = new(string.Empty);


        // Parse and write the report
        foreach (string line in lines)
        {
            StringBuilder sb = new(line);
            //Story Role section
            sb.Replace("@Id", character.Id.ToString());
            sb.Replace("@Title", character.Name);
            sb.Replace("@Role", character.Role);
            sb.Replace("@StoryRole", character.StoryRole);
            sb.Replace("@Archetype", character.Archetype);
            sb.Replace("@CharacterSketch", GetText(character.CharacterSketch));
            //Physical section
            sb.Replace("@Age", character.Age);
            sb.Replace("@Sex", character.Sex);
            sb.Replace("@Height", character.CharHeight);
            sb.Replace("@Weight", character.Weight);
            sb.Replace("@Eyes", character.Eyes);
            sb.Replace("@Hair", character.Hair);
            sb.Replace("@Build", character.Build);
            sb.Replace("@Skin", character.Complexion);
            sb.Replace("@Race", character.Race);
            sb.Replace("@Nationality", character.Nationality);
            sb.Replace("@Health", character.Health);
            sb.Replace("@PhysNotes", GetText(character.PhysNotes));
            //Appearance section
            sb.Replace("@Appearance", GetText(character.Appearance));
            //Relationships section
            sb.Replace("@Relationship", character.Relationship);
            sb.Replace("@relationType", character.RelationType);
            sb.Replace("@relationTrait", character.RelationTrait);
            sb.Replace("@Attitude", character.Attitude);
            sb.Replace("@RelationshipNotes", character.RelationshipNotes);
            //Flaw section
            sb.Replace("@Flaw", GetText(character.Flaw));
            //Backstory section
            sb.Replace("@Notes", GetText(character.BackStory));
            //Social Traits section
            sb.Replace("@Economic", GetText(character.Economic));
            sb.Replace("@Education", GetText(character.Education));
            sb.Replace("@Ethnic", GetText(character.Ethnic));
            sb.Replace("@Religion", GetText(character.Religion));
            //Psychological Traits section
            sb.Replace("@Personality", character.Enneagram);
            sb.Replace("@Intelligence", character.Intelligence);
            sb.Replace("@Values", character.Values);
            sb.Replace("@Focus", character.Focus);
            sb.Replace("@Abnormality", character.Abnormality);
            sb.Replace("@PsychNotes", GetText(character.PsychNotes));
            //Inner Traits section
            sb.Replace("@Adventure", character.Adventureousness);
            sb.Replace("@Aggression", character.Aggression);
            sb.Replace("@Confidence", character.Confidence);
            sb.Replace("@Conscientious", character.Conscientiousness);
            sb.Replace("@Creative", character.Creativity);
            sb.Replace("@Dominance", character.Dominance);
            sb.Replace("@Enthusiasm", character.Enthusiasm);
            sb.Replace("@Assurance", character.Assurance);
            sb.Replace("@Sensitivity", character.Sensitivity);
            sb.Replace("@Shrewdness", character.Shrewdness);
            sb.Replace("@Sociability", character.Sociability);
            sb.Replace("@Stability", character.Stability);
            //Outer Traits section
            sb.Replace("@Traits", character.outerTrait);
            // Notes section
            sb.Replace("@Notes", GetText(character.Notes));

            doc.AddText(sb.ToString());
            doc.AddNewLine();
        }
         
        return doc.GetRtf();
    }

    public string FormatSettingListReport()
    {
        string[] lines = _templates["List of Settings"];
        RtfDocument doc = new(string.Empty);

        // Parse and write the report
        foreach (string line in lines)
        {
            if (line.Contains("@Description"))
            {
                foreach (StoryElement element in _model.StoryElements)
                    if (element.Type == StoryItemType.Setting)
                    {
                        SettingModel setting = (SettingModel)element;
                        StringBuilder sb = new(line);
                        sb.Replace("@Description", setting.Name);
                        doc.AddText(sb.ToString());
                        doc.AddNewLine();
                    }
            }
            else
            {
                doc.AddText(line);
                doc.AddNewLine();
            }
        }
        return doc.GetRtf();
    }

    public string FormatSettingReport(StoryElement element)
    {
        SettingModel setting = (SettingModel)element;
        string[] lines = _templates["Setting Description"];
        RtfDocument doc = new(string.Empty);

        // Parse and write the report
        foreach (string line in lines)
        {
            StringBuilder sb = new(line);
            sb.Replace("@Id", setting.Id.ToString());
            sb.Replace("@Title", setting.Name);
            sb.Replace("@Locale", setting.Locale);
            sb.Replace("@Season", setting.Season);
            sb.Replace("@Period", setting.Period);
            sb.Replace("@Lighting", setting.Lighting);
            sb.Replace("@Weather", setting.Weather);
            sb.Replace("@Temperature", setting.Temperature);
            sb.Replace("@Prop1", setting.Prop1);
            sb.Replace("@Prop2", setting.Prop2);
            sb.Replace("@Prop3", setting.Prop3);
            sb.Replace("@Prop4", setting.Prop4);
            sb.Replace("@Summary", GetText(setting.Summary));
            sb.Replace("@Sights", GetText(setting.Sights));
            sb.Replace("@Sounds", GetText(setting.Sounds));
            sb.Replace("@Touch", GetText(setting.Touch));
            sb.Replace("@SmellTaste", GetText(setting.SmellTaste));
            sb.Replace("@Notes", GetText(setting.Notes));
            doc.AddText(sb.ToString());
            doc.AddNewLine();
        }
        return doc.GetRtf();
    }

    public string FormatSceneListReport()
    {
        string[] lines = _templates["List of Scenes"];
        RtfDocument doc = new(string.Empty);

        // Parse and write the report
        foreach (string line in lines)
        {
            if (line.Contains("@Description"))
            {
                foreach (StoryElement element in _model.StoryElements)
                    if (element.Type == StoryItemType.Scene)
                    {
                        SceneModel scene = (SceneModel)element;
                        StringBuilder sb = new(line);
                        sb.Replace("@Description", scene.Name);
                        doc.AddText(sb.ToString());
                        doc.AddNewLine();
                    }
            }
            else
            {
                doc.AddText(line);
                doc.AddNewLine();
            }
        }
        return doc.GetRtf(); 
    }

    public string FormatSceneReport(StoryElement element)
    {
        SceneModel scene = (SceneModel)element;
        string[] lines = _templates["Scene Description"];
        RtfDocument doc = new(string.Empty);

        StoryElement antagonist = StringToStoryElement(scene.Antagonist);
        string antagonistName = antagonist?.Name ?? string.Empty;
        StoryElement protagonist = StringToStoryElement(scene.Protagonist);
        string protagonistName = protagonist?.Name ?? string.Empty;
        StoryElement setting = StringToStoryElement(scene.Setting);
        string settingName = setting?.Name ?? string.Empty;

        // Parse and write the report
        foreach (string line in lines)
        {
            StringBuilder sb = new(line);
            //SCENE OVERVIEW SECTION
            sb.Replace("@Title", scene.Name);
            sb.Replace("@Date", scene.Date);
            sb.Replace("@Time", scene.Time);
            sb.Replace("@Viewpoint", scene.Viewpoint);
            sb.Replace("@Setting", settingName);
            sb.Replace("@SceneType", scene.SceneType);

            if (line.Contains("@CastMember"))
            {
                if(scene.CastMembers.Count == 0)
                {
                    sb.Replace("@CastMember", string.Empty);
                }
                else
                {
                    foreach (string seCastMember in scene.CastMembers)
                    {
                        StoryElement castMember = StringToStoryElement(seCastMember);
                        string castMemberName = castMember?.Name ?? string.Empty;
                        StringBuilder sbCast = new(line);
                            
                        sbCast.Replace("@CastMember", castMemberName);
                        doc.AddText(sbCast.ToString());
                        doc.AddNewLine();
                    }
                }
                    
            }

            sb.Replace("@Remarks", GetText(scene.Remarks));
            //DEVELOPMENT SECTION
            sb.Replace("@PurposeOfScene", scene.ScenePurpose);
            sb.Replace("@ValueExchange", scene.ValueExchange);
            sb.Replace("@Events", GetText(scene.Events));
            sb.Replace("@Consequence", GetText(scene.Consequences));
            sb.Replace("@Significance", GetText(scene.Significance));
            sb.Replace("@Realization", GetText(scene.Realization));
            //SCENE CONFLICT SECTION
            sb.Replace("@ProtagName", protagonistName);
            sb.Replace("@ProtagEmotion", scene.ProtagEmotion);
            sb.Replace("@ProtagGoal", scene.ProtagGoal);
            sb.Replace("@AntagName", antagonistName);
            sb.Replace("@AntagEmotion", scene.AntagEmotion);
            sb.Replace("@AntagGoal", scene.AntagGoal);
            sb.Replace("@Outcome", scene.Outcome);
            //SEQUEL SECTION
            sb.Replace("@Emotion", scene.Emotion);
            sb.Replace("@Review", GetText(scene.Review));
            sb.Replace("@NewGoal", scene.NewGoal);
            //SCENE NOTES SECTION
            sb.Replace("@Notes", GetText(scene.Notes));

            doc.AddText(sb.ToString());
            doc.AddNewLine();
        }

        return doc.GetRtf();
    }

    public string FormatFolderReport(StoryElement element)
    {
        FolderModel folder = (FolderModel)element;
        string[] lines = _templates["Folder Description"];
        RtfDocument doc = new(string.Empty);
        // Parse and write the report
        foreach (string line in lines)
        {
            StringBuilder sb = new(line);
            sb.Replace("@Name", folder.Name);
            sb.Replace("@Notes", GetText(folder.Notes));
            doc.AddText(sb.ToString());  //,format);
            doc.AddNewLine();
        }
        return doc.GetRtf();
    }

    public string FormatSectionReport(StoryElement element)
    {
        SectionModel section = (SectionModel)element;
        string[] lines = _templates["Section Description"];
        RtfDocument doc = new(string.Empty);

        // Parse and write the report
        foreach (string line in lines)
        {
            StringBuilder sb = new(line);
            sb.Replace("@Name", section.Name);
            sb.Replace("@Notes", GetText(section.Notes));
            doc.AddText(sb.ToString()); // , format);
            doc.AddNewLine();
        }
        return doc.GetRtf();
    }

    public string FormatSynopsisReport()
    {

        string[] lines = _templates["Story Synopsis"];
        RtfDocument doc = new(string.Empty);

        // Pre-process RTF properties to preserve [FILE:x.rtf] tag for long fields
        // and then load long fields from their corresponding file in its subfolder

        // Parse and write the report
        foreach (string line in lines)
        {
            if (line.Contains("@Synopsis"))
            {
                // Find StoryNarrator' Scenes
                foreach (StoryNodeItem child in _model.NarratorView[0].Children)
                {
                    StoryElement scn = _model.StoryElements.StoryElementGuids[child.Uuid];
                    if (scn.Type != StoryItemType.Scene)
                        continue;
                    SceneModel scene = (SceneModel)scn;
                    StringBuilder sb = new(line);
                    sb.Replace("@Synopsis", $"[{scene.Name}] {scene.Description}");
                    doc.AddText(sb.ToString());
                    doc.AddNewLine();
                    doc.AddText(scene.Remarks);
                    doc.AddNewLine();
                }
            }
            else
            {
                doc.AddText(line);
                doc.AddNewLine();
            }
        }
        return doc.GetRtf();
    }

    public async Task LoadReportTemplates()
    {
        try
        {
            _templates.Clear();
            StorageFolder localFolder = ApplicationData.Current.RoamingFolder;
            StorageFolder stbFolder = await localFolder.GetFolderAsync("StoryBuilder");
            StorageFolder templatesFolder = await stbFolder.GetFolderAsync("reports");
            IReadOnlyList<StorageFile> templates = await templatesFolder.GetFilesAsync();
            foreach (StorageFile fi in templates)
            {
                string name = fi.DisplayName[..(fi.Name.Length - 4)];
                string text = await FileIO.ReadTextAsync(fi);
                string[] lines = text.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                );
                _templates.Add(name, lines);
            }
        }
        catch (Exception ex)
        {
            Ioc.Default.GetService<LogService>().LogException(LogLevel.Error, ex, "Error loading report templates.");
        }
    }

    #endregion

    #region Private methods
    private StoryElement StringToStoryElement(string value)
    {
        if (value == null)
            return null;
        if (value.Equals(string.Empty))
            return null;
        // Get the current StoryModel's StoryElementsCollection
        ShellViewModel shell = Ioc.Default.GetService<ShellViewModel>();
        StoryElementCollection elements = shell.StoryModel.StoryElements;
        // legacy: locate the StoryElement from its Name
        foreach (StoryElement element in elements)  // Character or Setting??? Search both?
        {
            if (element.Type == StoryItemType.Character | element.Type == StoryItemType.Setting)
            {
                if (value.Equals(element.Name))
                    return element;
            }
        }
        // Look for the StoryElement corresponding to the passed guid
        // (This is the normal approach)
        if (Guid.TryParse(value, out Guid guid))
            if (elements.StoryElementGuids.ContainsKey(guid))
                return elements.StoryElementGuids[guid];
        return null;  // Not found
    }

    /// <summary>
    /// A RichEditBox property is an a wrapper for an RTF 
    /// document, with its header, font table, color table, etc.,
    /// and which can be read or written. This causes format problems
    /// when it's a cell on a StoryBuilder report.  This function
    /// returns only the text, but does preserve newlines as 
    /// paragraph breaks.
    /// </summary>
    public string GetText(string rtfInput, bool formatNewLines = true)
    {
        string text = rtfInput ?? string.Empty;
        if (rtfInput.Equals(string.Empty))
            return string.Empty;
        RichTextStripper rts = new();
        text =  rts.StripRichTextFormat(text);
        if (formatNewLines)
            text = text.Replace("\n","\\par");
        return text;
    }

    /// <summary>
    /// Generate a UUID in the Scrivener format (i.e., without curly braces)
    /// </summary>
    /// <returns>string UUID representation</returns>

    #endregion

    #region Constructor

    public ReportFormatter() 
    {
        _rdr = Ioc.Default.GetService<StoryReader>();
        ShellViewModel shell = Ioc.Default.GetService<ShellViewModel>();
        _model = shell.StoryModel;
    }

    #endregion
}