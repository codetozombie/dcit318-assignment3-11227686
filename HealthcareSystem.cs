using System;
using System.Collections.Generic;
using System.Linq;

// Question 2: Healthcare System using Generics and Collections

// Generic Repository class
public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(items);
    }

    public T GetById(int id)
    {
        return items.FirstOrDefault(item => 
        {
            var idProperty = typeof(T).GetProperty("Id");
            return idProperty != null && (int)idProperty.GetValue(item) == id;
        });
    }

    public bool Remove(T item)
    {
        return items.Remove(item);
    }
}

// Patient class
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string MedicalHistory { get; set; }

    public Patient(int id, string name, DateTime dateOfBirth, string medicalHistory)
    {
        Id = id;
        Name = name;
        DateOfBirth = dateOfBirth;
        MedicalHistory = medicalHistory;
    }

    public override string ToString()
    {
        return $"Patient ID: {Id}, Name: {Name}, DOB: {DateOfBirth:yyyy-MM-dd}, History: {MedicalHistory}";
    }
}

// Prescription class
public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string MedicationName { get; set; }
    public string Dosage { get; set; }
    public DateTime DatePrescribed { get; set; }

    public Prescription(int id, int patientId, string medicationName, string dosage, DateTime datePrescribed)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        Dosage = dosage;
        DatePrescribed = datePrescribed;
    }

    public override string ToString()
    {
        return $"Prescription ID: {Id}, Medication: {MedicationName}, Dosage: {Dosage}, Date: {DatePrescribed:yyyy-MM-dd}";
    }
}

// Health System Application
public class HealthSystemApp
{
    private Repository<Patient> patientRepository;
    private Repository<Prescription> prescriptionRepository;
    private Dictionary<int, List<Prescription>> prescriptionMap;

    public HealthSystemApp()
    {
        patientRepository = new Repository<Patient>();
        prescriptionRepository = new Repository<Prescription>();
        prescriptionMap = new Dictionary<int, List<Prescription>>();
    }

    public void SeedData()
    {
        // Add sample patients
        patientRepository.Add(new Patient(1, "John Doe", new DateTime(1990, 5, 15), "Diabetes"));
        patientRepository.Add(new Patient(2, "Jane Smith", new DateTime(1985, 8, 22), "Hypertension"));
        patientRepository.Add(new Patient(3, "Bob Johnson", new DateTime(1978, 12, 3), "Asthma"));

        // Add sample prescriptions
        prescriptionRepository.Add(new Prescription(1, 1, "Metformin", "500mg twice daily", DateTime.Now.AddDays(-10)));
        prescriptionRepository.Add(new Prescription(2, 1, "Insulin", "10 units before meals", DateTime.Now.AddDays(-5)));
        prescriptionRepository.Add(new Prescription(3, 2, "Lisinopril", "10mg once daily", DateTime.Now.AddDays(-7)));
        prescriptionRepository.Add(new Prescription(4, 3, "Albuterol", "2 puffs as needed", DateTime.Now.AddDays(-3)));
    }

    public void BuildPrescriptionMap()
    {
        prescriptionMap.Clear();
        var allPrescriptions = prescriptionRepository.GetAll();
        
        foreach (var prescription in allPrescriptions)
        {
            if (!prescriptionMap.ContainsKey(prescription.PatientId))
            {
                prescriptionMap[prescription.PatientId] = new List<Prescription>();
            }
            prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public List<Prescription> GetPrescriptionsByPatientId(int patientId)
    {
        return prescriptionMap.ContainsKey(patientId) ? prescriptionMap[patientId] : new List<Prescription>();
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("=== All Patients ===");
        var patients = patientRepository.GetAll();
        foreach (var patient in patients)
        {
            Console.WriteLine(patient);
        }
        Console.WriteLine();
    }

    public void PrintPrescriptionsForPatient(int patientId)
    {
        var patient = patientRepository.GetById(patientId);
        if (patient != null)
        {
            Console.WriteLine($"=== Prescriptions for {patient.Name} ===");
            var prescriptions = GetPrescriptionsByPatientId(patientId);
            foreach (var prescription in prescriptions)
            {
                Console.WriteLine(prescription);
            }
        }
        else
        {
            Console.WriteLine($"Patient with ID {patientId} not found.");
        }
        Console.WriteLine();
    }

    public static void Main()
    {
        Console.WriteLine("=== Healthcare System ===\n");
        
        var app = new HealthSystemApp();
        app.SeedData();
        app.BuildPrescriptionMap();
        
        app.PrintAllPatients();
        app.PrintPrescriptionsForPatient(1);
        app.PrintPrescriptionsForPatient(2);
        app.PrintPrescriptionsForPatient(3);
    }
}