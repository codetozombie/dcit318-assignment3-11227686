using System;
using System.Collections.Generic;

// Question 1: Finance Management System

// Record for Transaction
public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

// Interface for transaction processing
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// Concrete implementations of transaction processors
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Processing Bank Transfer: ID {transaction.Id}, Amount: ${transaction.Amount:F2}, Category: {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Processing Mobile Money: ID {transaction.Id}, Amount: ${transaction.Amount:F2}, Category: {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Processing Crypto Wallet: ID {transaction.Id}, Amount: ${transaction.Amount:F2}, Category: {transaction.Category}");
    }
}

// Base Account class
public class Account
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance += transaction.Amount;
        Console.WriteLine($"Transaction applied to account {AccountNumber}. New balance: ${Balance:F2}");
    }
}

// Sealed SavingsAccount class
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance) 
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        // Prevent overdrafts
        if (Balance + transaction.Amount < 0)
        {
            Console.WriteLine($"Transaction rejected: Insufficient funds. Current balance: ${Balance:F2}");
            return;
        }
        
        base.ApplyTransaction(transaction);
    }
}

// Main Finance Application
public class FinanceApp
{
    private List<Transaction> transactions;
    private List<ITransactionProcessor> processors;
    private SavingsAccount savingsAccount;

    public FinanceApp()
    {
        transactions = new List<Transaction>();
        processors = new List<ITransactionProcessor>
        {
            new BankTransferProcessor(),
            new MobileMoneyProcessor(),
            new CryptoWalletProcessor()
        };
        savingsAccount = new SavingsAccount("SAV001", 1000.00m);
    }

    public void Run()
    {
        Console.WriteLine("=== Finance Management System ===\n");
        
        // Add sample transactions
        transactions.Add(new Transaction(1, DateTime.Now, 500.00m, "Salary"));
        transactions.Add(new Transaction(2, DateTime.Now, -200.00m, "Groceries"));
        transactions.Add(new Transaction(3, DateTime.Now, -1500.00m, "Rent")); // This should be rejected

        // Process transactions
        for (int i = 0; i < transactions.Count; i++)
        {
            var transaction = transactions[i];
            var processor = processors[i % processors.Count];
            
            processor.Process(transaction);
            savingsAccount.ApplyTransaction(transaction);
            Console.WriteLine();
        }
    }

    public static void Main()
    {
        var app = new FinanceApp();
        app.Run();
    }
}
