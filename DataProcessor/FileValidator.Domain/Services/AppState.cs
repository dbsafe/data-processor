﻿namespace FileValidator.Domain.Services
{
    public class AppState
    {
        public string AppName { get; } = "File Validator";
        public HomePageState HomePage { get; } = new HomePageState();
        public FileSpecificationsPageState FileSpecificationsPage { get; } = new FileSpecificationsPageState();
        public LoadedFilePageState LoadedFilePage { get; } = new LoadedFilePageState();
        public LoadedFileJsonPageState LoadedFileJsonPage { get; } = new LoadedFileJsonPageState();
    }

    public class HomePageState
    {
        public string InputDataContent { get; set; }
        public int SelectedFileSpecId { get; set; }
        public CursorPosition CursorPosition { get; set; }
    }

    public class CursorPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class FileSpecificationsPageState
    {
        public FileSpecification SelectedFileSpecification { get; set; }
        public CursorPosition CursorPosition { get; set; }
    }

    public class LoadedFilePageState
    {
        public ParsedDataAndSpec ParsedDataAndSpec { get; set; }
        public CursorPosition CursorPosition { get; set; }
    }

    public class LoadedFileJsonPageState
    {
        public CursorPosition CursorPosition { get; set; }
    }
}