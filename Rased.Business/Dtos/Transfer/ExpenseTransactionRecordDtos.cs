using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class ExpenseTransactionRecordDtos
    {
        public int TransactionId { get; set; }
        public int ExpenseId { get; set; }
        public DateTime CreatedAt { get; set; }
    }


    //public class AddPersonalIncomeTransactionRecordDto
    //{
    //    public int TransactionId { get; set; }
    //    public int IncomeId { get; set; }
    //    public int ApprovalId { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //}

    //public class AddSharedWalletIncomeTransactionDto
    //{
    //    public int TransactionId { get; set; }
    //    public int IncomeId { get; set; }
    //    public int ApprovalId { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //}

    public class GetExpenseTransactionRecordDto
    {
        public int ExpenseTrasactionRecordId { get; set; }
        public int TransactionId { get; set; }
        public int ExpenseId { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ExpenseSpecificData { get; set; }
    }


    public class UpdateExpenseTransactionRecordDto
    {
        public int? ExpenseId { get; set; } 
        public string? ExpenseSpecificData { get; set; } 
    }


    //public class GetPersonalIncomeTransactionRecordDto
    //{
    //    public int PersonalIncomeTrasactionRecordId { get; set; }
    //    public int TransactionId { get; set; }
    //    public int IncomeId { get; set; }
    //    public int ApprovalId { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //}

    //public class GetSharedWalletIncomeTransactionDto
    //{
    //    public int SharedWalletIncomeTransactionId { get; set; }
    //    public int TransactionId { get; set; }
    //    public int IncomeId { get; set; }
    //    public int ApprovalId { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //}





}
