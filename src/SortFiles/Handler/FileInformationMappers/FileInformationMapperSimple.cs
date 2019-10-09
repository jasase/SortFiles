using System.Collections.Generic;
using System.Linq;
using SortFilesPlugin.FileInformationExtractors;
using SortFilesPlugin.Handler.FileInformationMappers.Configuration;

namespace SortFilesPlugin.Handler.FileInformationMappers
{
    public class FileInformationMapperSimple : FileInformationMapper
    {
        private readonly FileInformationMapperSimpleConfiguration _configuration;
        private readonly Dictionary<string, Dictionary<string, string>> _mapping;

        public FileInformationMapperSimple(FileInformationMapperSimpleConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
            _mapping = BuildMapping();

        }

        private Dictionary<string, Dictionary<string, string>> BuildMapping()
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            foreach (var mappingGroup in _configuration.Mappings ?? new FileInformationMapperSimpleKeyMappingGroup[0])
            {
                var group = new Dictionary<string, string>();
                foreach (var map in mappingGroup.Map ?? new FileInformationMapperSimpleKeyMapping[0])
                {
                    group.Add(map.Source, map.Result);
                }
                result.Add(mappingGroup.Key, group);
            }
            return result;
        }

        public override FileInformation MapInformation(FileInformation information)
        {
            foreach (var cur in information.ToArray())
            {
                if (_mapping.ContainsKey(cur.Key) &&
                   _mapping[cur.Key].ContainsKey(cur.Value.ToString()))
                {
                    var newKey = cur.Key + "Mapped";
                    information.AddInformation(newKey, _mapping[cur.Key][cur.Value.ToString()]);
                }
            }

            return information;
        }
    }
}
