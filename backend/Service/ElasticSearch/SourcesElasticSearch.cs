using backend.Data;
using backend.Dtos;
using backend.Entities;
using Nest;
using System.Net.WebSockets;

namespace backend.Service.ElasticSearch
{
    public class SourcesElasticSearch : ISourcesElasticSearch
    {
        private readonly IElasticSearchRepository elasticSearchRepository;
        public SourcesElasticSearch(IElasticSearchRepository _elasticSearchRepository)
        {
            elasticSearchRepository = _elasticSearchRepository;
        }
        public bool AddSources(string topicId, SourceDto source)
        {
            var updateResponse = elasticSearchRepository.UpdateData(topicId,
                u => u.Index("sources_index")
                      .Script(s => s
                          .Source(@"
                      for (int i = 0; i < ctx._source.subTopics.size(); i++) { 
                          if (ctx._source.subTopics[i].SubTopicId == params.subTopicId) { 
                              ctx._source.subTopics[i].sources.add(params.source); 
                          } 
                      }")
                          .Params(p => p
                              .Add("subTopicId", source.SubTopicId)
                              .Add("source", source)
                          )
                      )
            );

            return updateResponse;
        }

        public bool RemoveSources(string topicId, string subTopicId, string sourceId)
        {
            var removeResponse = elasticSearchRepository.UpdateData(topicId, u => u
                .Index("sources_index")
                .Script(s => s
                    .Source(@"
                for (int i = 0; i < ctx._source.subTopics.size(); i++) { 
                    if (ctx._source.subTopics[i].SubTopicId == params.subTopicId) { 
                        for (int j = 0; j < ctx._source.subTopics[i].sources.size(); j++) {
                            if (ctx._source.subTopics[i].sources[j].Id == params.sourceId) {
                                ctx._source.subTopics[i].sources.remove(j);
                                break;
                            }
                        }
                    } 
                }")
                    .Params(p => p
                        .Add("subTopicId", subTopicId)
                        .Add("sourceId", sourceId)
                    )
                )
            );

            return removeResponse;
        }
        public bool UpdateData(string topicId, SourceDto source)
        {
            var updateResponse = elasticSearchRepository.UpdateData(topicId, u => u
                .Index("sources_index")
                .Script(s => s
                    .Source(@"
                for (int i = 0; i < ctx._source.subTopics.size(); i++) {
                    if (ctx._source.subTopics[i].SubTopicId == params.SubTopicId) {
                        for (int j = 0; j < ctx._source.subTopics[i].sources.size(); j++) {
                            if (ctx._source.subTopics[i].sources[j].Id == params.id) {
                                ctx._source.subTopics[i].sources[j].Title = params.Title;
                                ctx._source.subTopics[i].sources[j].Description = params.Description;
                                ctx._source.subTopics[i].sources[j].Thumbnail = params.Thumbnail;
                                ctx._source.subTopics[i].sources[j].Slug = params.Slug;
                                ctx._source.subTopics[i].sources[j].Status = params.Status;
                                ctx._source.subTopics[i].sources[j].Benefit = params.Benefit;
                                ctx._source.subTopics[i].sources[j].Video_intro = params.Video_intro;
                                ctx._source.subTopics[i].sources[j].Price = params.Price;
                                ctx._source.subTopics[i].sources[j].Rating = params.Rating;
                                ctx._source.subTopics[i].sources[j].UserId = params.UserId;
                                break;
                            }
                        }
                        break;
                    }
                }")
                    .Params(p => p
                        .Add("SubTopicId", source.SubTopicId)
                        .Add("id", source.Id)
                        .Add("Title", source.Title)
                        .Add("Description", source.Description)
                        .Add("Thumbnail", source.Thumbnail)
                        .Add("Slug", source.Slug)
                        .Add("Status", source.Status)
                        .Add("Benefit", source.Benefit)
                        .Add("Video_intro", source.VideoIntro)
                        .Add("Price", source.Price)
                        .Add("Rating", source.Rating)
                        .Add("UserId", source.UserId)
                    )
                )
            );

            return updateResponse;
        }



    }
}
