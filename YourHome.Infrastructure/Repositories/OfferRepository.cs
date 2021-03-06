﻿using AutoMapper;
using Nest;
using System.Collections.Generic;
using YourHome.Core.Abstract;
using YourHome.Core.Models.Domain;
using YourHome.Infrastructure.Enums;
using Offer = YourHome.Infrastructure.Models.Offer;

namespace YourHome.Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private const int PageSize = 20;

        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public OfferRepository(IElasticClient elasticClient, IMapper mapper)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        public Core.Models.Domain.Offer Get(string id)
        {
            var response = _elasticClient.Get<Offer>(id);
            var offer = _mapper.Map<Core.Models.Domain.Offer>(response.Source);
            return offer;
        }

        public void Add(Core.Models.Domain.Offer offer)
        {
            var infrastructureOffer = _mapper.Map<Core.Models.Domain.Offer>(offer);
            _elasticClient.Index(infrastructureOffer, i => i);
        }

        public IEnumerable<Core.Models.Domain.Offer> Search(SearchArguments searchArguments)
        {
            var searchResponse = _elasticClient.Search<Offer>(s => s
            .Query(q => q
                .Bool(b => b
                    .Must(mu => mu
                        .Match(m => m
                            .Field(f => f.Title)
                            .Query(searchArguments.SearchPhrase)
                        ), mu => mu
                        .Match(m => m
                            .Field(f => f.Description)
                            .Query(searchArguments.SearchPhrase)
                        )
            )
            .Filter(fi => fi.Range(r => r
                    .Field(f => f.Price)
                    .GreaterThanOrEquals((double?)searchArguments.MinPrice)
                    .LessThanOrEquals((double?)searchArguments.MaxPrice))
                        ,
                    fi => fi.Range(r => r
                        .Field(f => f.Area)
                        .GreaterThanOrEquals(searchArguments.MinArea)
                        .LessThanOrEquals(searchArguments.MaxArea))
                        ,
                    fi => fi.Range(r => r
                            .Field(f => f.RoomCount)
                            .GreaterThanOrEquals(searchArguments.MinRoomCount)
                            .LessThanOrEquals(searchArguments.MaxRoomCount))
                        ,
                    bs => bs.Term(p => p.Market, searchArguments.Market)
                        ,
                    bs => bs.Term(p => p.OfferType, searchArguments.OfferType)

            )))
            .From((searchArguments.Page - 1) * PageSize)
            .Size(PageSize));

            var offers = _mapper.Map<IEnumerable<Core.Models.Domain.Offer>>(searchResponse.Documents);
            return offers;
        }

        public void Activate(string offerId)
        {
            var response = _elasticClient.Get<Offer>(offerId);
            var offer = _mapper.Map<Core.Models.Domain.Offer>(response.Source);
            var infrastructureOffer = _mapper.Map<Core.Models.Domain.Offer>(offer);
            infrastructureOffer.State = (int)StateOffer.Active;
            _elasticClient.Index(infrastructureOffer, i => i);
        }
    }
}
