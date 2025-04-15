using AutoMapper;
using Rased.Business.Dtos.Transfer;
using Rased.Infrastructure;
using Rased.Infrastructure.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Rased.Business.AutoMapper
{
   public class TransferProfile : Profile
    {
        public TransferProfile()
        {
            CreateMap<Infrastructure.Transaction, ReadTransactionForSenderDto>().ReverseMap();
            CreateMap<Infrastructure.Transaction, ReadTransactionForReceiverDto>().ReverseMap();
            CreateMap<Infrastructure.Transaction,ReadTransactionDto>().ReverseMap();
            CreateMap<Infrastructure.Transaction, AddTransactionDto>().ReverseMap();
            CreateMap<Infrastructure.Transaction, UpdateTransactionDto>().ReverseMap();

            CreateMap<StaticTransactionStatusData, AddStaticTransactionStatusDto>().ReverseMap();
            CreateMap<StaticTransactionStatusData, UpdateStaticTransactionStatusDto>().ReverseMap();
            CreateMap<StaticTransactionStatusData, ReadStaticTransactionStatusDto>().ReverseMap();


            CreateMap<StaticReceiverTypeData, ReadStaticReceiverTypeDataDto>().ReverseMap();
            CreateMap<StaticReceiverTypeData, UpdateStaticReceiverTypeDataDto>().ReverseMap();
            CreateMap<StaticReceiverTypeData, AddStaticReceiverTypeDataDto>().ReverseMap();


            CreateMap<TransactionRejection, ReadTransactionRejectionDto>().ReverseMap();
            CreateMap<TransactionRejection, AddTransactionRejectionDto>().ReverseMap();
            CreateMap<TransactionRejection, UpdateTransactionRejectionDto>().ReverseMap();


            CreateMap<ExpenseTransactionRecord, GetExpenseTransactionRecordDto>().ReverseMap();
            CreateMap<PersonalIncomeTrasactionRecord, GetPersonalIncomeTransactionRecordDto>().ReverseMap();
            CreateMap<SharedWalletIncomeTransaction, GetSharedWalletIncomeTransactionDto>().ReverseMap();



        }
    }
}
