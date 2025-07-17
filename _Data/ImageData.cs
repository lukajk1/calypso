using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    public class ImageData
    {
        public string FullResPath {  get; set; }
        public string ThumbnailPath {  get; set; }
        public string Filename {  get; set; }
        public List<string> Tags { get; set; }

        public ImageData(string fullResPath, string thumbnailPath, string filename)
        {
            FullResPath = fullResPath;
            ThumbnailPath = thumbnailPath;
            Filename = filename;
            Tags = new List<string>();
        }

        public void SetTagList(List<string> tags)
        {
            Tags = tags;
            AlphabetizeTags();
        }

        public void AddTags(List<string> tags)
        {
            foreach (var t in tags)
            {
                if (!Tags.Contains(t))
                {
                    Tags.Add(t);
                }
            }
            AlphabetizeTags();
        }

        public void AddTag(string tag)
        {
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }

        public void RemoveTags(List<string> tags)
        {
            foreach (var t in tags)
            {
                if (Tags.Contains(t))
                {
                    Tags.Remove(t);
                }
            }
        }
        public void RemoveTag(string tag)
        {
            if (Tags.Contains(tag))
            {
                Tags.Remove(tag);
            }
        }

        public void AlphabetizeTags()
        {
            Tags = Tags.OrderBy(t => t).ToList();
        }

        public ImageData() { }
    }
}
