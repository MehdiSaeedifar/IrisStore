using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Iris.DomainClasses;
using Iris.ViewModels;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Similar;
using Lucene.Net.Store;
using SpellChecker.Net.Search.Spell;
using Utilities;
using Directory = System.IO.Directory;
using ParseException = Lucene.Net.QueryParsers.ParseException;
using Version = Lucene.Net.Util.Version;

namespace Iris.LuceneSearch
{
    public class LuceneIndex
    {
        private const Version _version = Version.LUCENE_30;

        private static readonly string _luceneDir = HttpRuntime.AppDomainAppPath + @"App_Data\Lucene_Index";


        private static FSDirectory _directory
        {
            get
            {
                //if (_directoryTemp == null)
                var directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                //if (IndexWriter.IsLocked(_directoryTemp))
                //    IndexWriter.Unlock(_directoryTemp);
                //string lockFilePath = Path.Combine(_luceneDir, "write.lock");
                //if (File.Exists(lockFilePath))
                //    File.Delete(lockFilePath);
                return directoryTemp;
            }
        }

        private static void _addToLuceneIndex(LuceneSearchModel modelData, IndexWriter writer)
        {
            // remove older index entry

            TermQuery searchQuery;

            if (modelData.PostId.HasValue)
            {
                searchQuery = new TermQuery(new Term(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.PostId), modelData.PostId.Value.ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                searchQuery = new TermQuery(new Term(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductId), modelData.ProductId.Value.ToString(CultureInfo.InvariantCulture)));
            }


            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var document = new Document();

            // add lucene fields mapped to db fields

            if (modelData.PostId.HasValue)
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.PostId),
              modelData.PostId.Value.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
            }
            else
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductId),
               modelData.ProductId.Value.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
            }

            if (modelData.Title != null)
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Title), modelData.Title, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS)
                {
                    Boost = 3
                });
            }


            if (modelData.Description != null)
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Description), modelData.Description, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS));
            }

            if (modelData.SlugUrl != null)
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.SlugUrl), modelData.SlugUrl, Field.Store.YES, Field.Index.NOT_ANALYZED));
            }

            if (modelData.Image != null)
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Image), modelData.Image, Field.Store.YES, Field.Index.NOT_ANALYZED));
            }

            if (modelData.Category != null)
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Category), modelData.Category, Field.Store.YES, Field.Index.NOT_ANALYZED));
            }

            if (modelData.Price != null)
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Price), modelData.Price, Field.Store.YES, Field.Index.NOT_ANALYZED));
            }

            if (modelData.ProductStatus != null)
            {
                document.Add(new Field(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductStatus), modelData.ProductStatus, Field.Store.YES, Field.Index.NOT_ANALYZED));
            }

            // add entry to index
            writer.AddDocument(document);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<LuceneSearchModel> modelData)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(_version);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entry if any)
                foreach (var data in modelData)
                    _addToLuceneIndex(data, writer);

                // close handles
                analyzer.Close();
                writer.Optimize();
                writer.Commit();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(LuceneSearchModel modelData)
        {
            AddUpdateLuceneIndex(new List<LuceneSearchModel> { modelData });
        }

        public static void ClearLuceneIndexRecord(int projectId)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(_version);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductId), projectId.ToString(CultureInfo.InvariantCulture)));


                // remove older index entry
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void ClearLucenePostIndexRecord(int postId)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(_version);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.PostId), postId.ToString(CultureInfo.InvariantCulture)));


                // remove older index entry
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        private static LuceneSearchModel _mapLuceneDocumentToData(Document doc)
        {
            return new LuceneSearchModel
            {
                PostId = Convert.ToInt32(doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.PostId))),
                ProductId = Convert.ToInt32(doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductId))),
                Description = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Description)),
                Title = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Title)),
                Image = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Image)),
                Category = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Category)),
                SlugUrl = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.SlugUrl)),
                ProductStatus = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductStatus)),
                Price = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Price))
            };
        }

        private static IEnumerable<LuceneSearchModel> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }

        private static IEnumerable<LuceneSearchModel> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private static IEnumerable<LuceneSearchModel> _search(string searchQuery, string[] searchFields)
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<LuceneSearchModel>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                const int hitsLimit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);


                var parser = new MultiFieldQueryParser
                    (Version.LUCENE_30, searchFields, analyzer);
                Query query = parseQuery(searchQuery, parser);
                ScoreDoc[] hits = searcher.Search(query, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;

                if (hits.Length == 0)
                {
                    searchQuery = searchByPartialWords(searchQuery);
                    query = parseQuery(searchQuery, parser);
                    hits = searcher.Search(query, hitsLimit).ScoreDocs;
                }

                IEnumerable<LuceneSearchModel> results = _mapLuceneToDataList(hits, searcher);
                analyzer.Close();
                searcher.Dispose();
                return results;
            }
        }

        public static IEnumerable<LuceneSearchModel> Search(string input, params string[] fieldsName)
        {
            if (string.IsNullOrEmpty(input))
                return new List<LuceneSearchModel>();

            IEnumerable<string> terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);
            return _search(input, fieldsName);
        }

        public static IEnumerable<LuceneSearchModel> SearchDefault(string input, string[] fieldsName)
        {
            return string.IsNullOrEmpty(input) ? new List<LuceneSearchModel>() : _search(input, fieldsName);
        }

        public static IEnumerable<LuceneSearchModel> GetAllIndexRecords()
        {
            // validate search index
            if (!Directory.EnumerateFiles(_luceneDir).Any())
                return new List<LuceneSearchModel>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory, false);
            IndexReader reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            TermDocs term = reader.TermDocs();
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }

        private static string searchByPartialWords(string bodyTerm)
        {
            bodyTerm = bodyTerm.Replace("*", "").Replace("?", "");
            IEnumerable<string> terms = bodyTerm.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Trim() + "*");
            bodyTerm = string.Join(" ", terms);
            return bodyTerm;
        }

        public static string[] SuggestSilmilarWords(string term, int count = 10)
        {
            IndexReader indexReader = IndexReader.Open(FSDirectory.Open(_luceneDir), true);

            // Create the SpellChecker
            var spellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(_luceneDir + "\\Spell"));

            // Create SpellChecker Index
            spellChecker.ClearIndex();
            spellChecker.IndexDictionary(new LuceneDictionary(indexReader, StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Title)));
            spellChecker.IndexDictionary(new LuceneDictionary(indexReader, StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Description)));

            //Suggest Similar Words
            return spellChecker.SuggestSimilar(term, count, null, null, true);
        }

        private static int GetLucenePostDocumentNumber(int postId)
        {
            var analyzer = new StandardAnalyzer(_version);
            var parser = new QueryParser(_version, StronglyTyped.PropertyName<LuceneSearchModel>(x => x.PostId), analyzer);
            Query query = parser.Parse(postId.ToString(CultureInfo.InvariantCulture));
            using (var searcher = new IndexSearcher(_directory, false))
            {
                TopDocs doc = searcher.Search(query, 1);

                return doc.TotalHits == 0 ? 0 : doc.ScoreDocs[0].Doc;
            }
        }

        private static Query CreateMoreLikeThisQuery(int postId)
        {
            int docNum = GetLucenePostDocumentNumber(postId);
            if (docNum == 0)
                return null;
            var analyzer = new StandardAnalyzer(_version);
            using (var searcher = new IndexSearcher(_directory, false))
            {
                IndexReader reader = searcher.IndexReader;
                var moreLikeThis = new MoreLikeThis(reader) { Analyzer = analyzer };
                moreLikeThis.SetFieldNames(new[]
                {
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.PostId),
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Title),
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Description),
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Category)
                });
                moreLikeThis.MinDocFreq = 1;
                moreLikeThis.MinTermFreq = 1;
                moreLikeThis.Boost = true;
                return moreLikeThis.Like(docNum);
            }
        }

        public static IList<ProductWidgetViewModel> GetMoreLikeThisPostItems(int postId)
        {
            Query query = CreateMoreLikeThisQuery(postId);
            if (query == null)
                return new List<ProductWidgetViewModel>();
            using (var searcher = new IndexSearcher(_directory, false))
            {
                TopDocs hits = searcher.Search(query, 10);
                return hits.ScoreDocs.Select(item => searcher.Doc(item.Doc)).Select(doc => new ProductWidgetViewModel
                {
                    Name = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Title)),
                    Id = Convert.ToInt32(doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.PostId))),
                    Image = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Image)),
                    Category = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Category)),
                    SlugUrl = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.SlugUrl))
                }).ToList();
            }
        }

        public static IList<ProductWidgetViewModel> GetMoreLikeThisProjectItems(int projectId)
        {
            Query query = CreateMoreProjectsLikeThisQuery(projectId);
            if (query == null)
                return new List<ProductWidgetViewModel>();
            using (var searcher = new IndexSearcher(_directory, false))
            {

                TopDocs hits = searcher.Search(query, 10);
                var docs = hits.ScoreDocs.Select(item => searcher.Doc(item.Doc)).ToList();

                return (from doc in docs
                        where !string.IsNullOrEmpty(doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductStatus)))
                        select new ProductWidgetViewModel
                        {
                            Name = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Title)),
                            Id = Convert.ToInt32(doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductId))),
                            Image = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Image)),
                            SlugUrl = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.SlugUrl)),
                            ProductStatus = (ProductStatus)Enum.Parse(typeof(ProductStatus), doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductStatus)), true),
                            Category = doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Category)),
                            Price = decimal.Parse(doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Price)))
                        }).ToList();
            }
        }

        private static Query CreateMoreProjectsLikeThisQuery(int projectId)
        {
            int docNum = GetLuceneProjectDocumentNumber(projectId);
            if (docNum == 0)
                return null;
            var analyzer = new StandardAnalyzer(_version);
            using (var searcher = new IndexSearcher(_directory, false))
            {
                IndexReader reader = searcher.IndexReader;
                var moreLikeThis = new MoreLikeThis(reader) { Analyzer = analyzer };
                moreLikeThis.SetFieldNames(new[]
                {
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductId),
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Title),
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Description),
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Price),
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductStatus),
                    StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Category)
                });
                moreLikeThis.MinDocFreq = 1;
                moreLikeThis.MinTermFreq = 1;
                moreLikeThis.Boost = true;
                return moreLikeThis.Like(docNum);
            }
        }

        private static int GetLuceneProjectDocumentNumber(int projectId)
        {
            var analyzer = new StandardAnalyzer(_version);
            var parser = new QueryParser(_version, StronglyTyped.PropertyName<LuceneSearchModel>(x => x.ProductId), analyzer);
            Query query = parser.Parse(projectId.ToString(CultureInfo.InvariantCulture));
            using (var searcher = new IndexSearcher(_directory, false))
            {
                TopDocs doc = searcher.Search(query, 1);

                return doc.TotalHits == 0 ? 0 : doc.ScoreDocs[0].Doc;
            }
        }


    }
}
