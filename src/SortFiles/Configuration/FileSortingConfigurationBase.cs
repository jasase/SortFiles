using SortFilesPlugin.Handler.FileContentChangers.Configuration;
using SortFilesPlugin.Handler.FileIndexers.Configuration;
using SortFilesPlugin.Handler.FileInformationFilters.Configuration;
using SortFilesPlugin.Handler.FileInformationMappers.Configuration;
namespace SortFilesPlugin.Configuration
{
    public abstract class FileSortingConfigurationBase
    {
        public string Name { get; set; }
    }

    public abstract class FileSortingConfigurationBase<T> : FileSortingConfigurationBase
        where T : FileSortingHandlerBase
    {
        public abstract T CreateHandler(IFileSortingHandlerCreator creator);
    }

    public interface IFileSortingHandlerCreator
    {
        T CreateHandler<T>(FileSortingConfigurationBase config)
            where T : FileSortingHandlerBase;
    }

    public class FileSortingConfiguration
    {
        public string Name { get; set; }
        public FileFinderConfiguration[] Finder { get; set; }

        public FileFilterConfiguration[] Filter { get; set; }
        public FileInformationExtractorConfiguration[] InformationExtractor { get; set; }
        public FileInformationMapperConfiguration[] InformationMapper { get; set; }
        public FileInformationFilterConfiguration[] InformationFilter { get; set; }
        public FilePathCreatorConfiguration[] Creator { get; set; }
        public FileMoverConfiguration[] Mover { get; set; }
        public FileContentChangerConfiguration[] ContentChanger { get; set; }
        public FileIndexerConfiguration[] Indexer { get; set; }
        public FileSortingRule[] Rules { get; set; }
    }

    public class FileSortingRule
    {
        public int Priority { get; set; }
        public string[] FinderNames { get; set; }
        public string[] FilterNames { get; set; }
        public string[] InformationExtractorNames { get; set; }
        public string[] InformationMapperNames { get; set; }
        public string[] InformationFilterNames { get; set; }
        public string PathCreatorName { get; set; }
        public string BackupPathCreatorName { get; set; }
        public string MoverName { get; set; }
        public string[] FileContentChanger { get; set; }
        public string[] Indexer { get; set; }

        public FileSortingRule()
        {
            Priority = 0;
            FinderNames = new string[0];
            FilterNames = new string[0];
            InformationExtractorNames = new string[0];
            InformationMapperNames = new string[0];
            FileContentChanger = new string[0];
            Indexer = new string[0];
        }
    }
}