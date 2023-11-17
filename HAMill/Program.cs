namespace HAMill
{
    internal class Program
    {
        public class CopperSmeltingControlSystem
        {
            public bool IsRunning { get; private set; }

            public event EventHandler TemperatureExceededThreshold;

            public void Start()
            {
                if (!IsRunning)
                {
                    IsRunning = true;
                    Console.WriteLine("Copper smelting control system is started.");
                }
            }

            public void Stop()
            {
                if (IsRunning)
                {
                    IsRunning = false;
                    Console.WriteLine("Copper smelting control system is stopped.");
                }
            }

            public void CheckTemperature(ITemperatureSensor temperatureSensor, double threshold)
            {
                if (IsRunning)
                {
                    double currentTemperature = temperatureSensor.GetTemperature();
                    if (currentTemperature > threshold)
                    {
                        OnTemperatureExceededThreshold(EventArgs.Empty);
                    }
                }
            }

            public virtual void OnTemperatureExceededThreshold(EventArgs e)
            {
                TemperatureExceededThreshold?.Invoke(this, e);
            }
        }



        public class CopperFurnace
        {
            public double Temperature { get; private set; }

            public void MeltCopper()
            {
                Temperature = 1084;
                Console.WriteLine("Copper smelting process is started in the furnace.");
            }

            public void StopMelting()
            {
                Temperature = 0;
                Console.WriteLine("Copper smelting process is stopped in the furnace.");
            }
        }

        public interface ITemperatureSensor
        {
            double GetTemperature();
        }

        public class CopperTemperatureSensor : ITemperatureSensor
        {
            private Random _random;
            private const double MinTemperature = 900;
            private const double MaxTemperature = 1200;

            public CopperTemperatureSensor()
            {
                _random = new Random();
            }

            public double GetTemperature()
            {
                double temperature = _random.NextDouble() * (MaxTemperature - MinTemperature) + MinTemperature;
                return temperature;
            }
        }
        static void ControlSystem_TemperatureExceededThreshold(object sender, EventArgs e)
        {
            Console.WriteLine("Temperature exceeded the threshold.");
        }

        public class Employee
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public double Salary { get; set; }
        }

        public class EmployeeManagementSystem
        {
            private List<Employee> employees;

            public EmployeeManagementSystem()
            {
                employees = new List<Employee>();
            }

            public void AddEmployee(Employee employee)
            {
                employees.Add(employee);
            }

            public void RemoveEmployee(string name)
            {
                employees.RemoveAll(e => e.Name == name);
            }

            public void PrintAllEmployees()
            {
                foreach (var employee in employees)
                {
                    Console.WriteLine("Name: {0}, Position: {1}, Salary: {2}", employee.Name, employee.Position, employee.Salary);
                }
            }

            public double CalculateTotalSalary()
            {
                double totalSalary = 0;
                foreach (var employee in employees)
                {
                    totalSalary += employee.Salary;
                }
                return totalSalary;
            }

            public double CalculateAverageSalary()
            {
                if (employees.Count == 0)
                {
                    return 0;
                }

                double totalSalary = CalculateTotalSalary();
                return totalSalary / employees.Count;
            }

            public List<Employee> FindEmployeesWithHigherSalary(double minSalary)
            {
                List<Employee> result = new List<Employee>();
                foreach (var employee in employees)
                {
                    if (employee.Salary > minSalary)
                    {
                        result.Add(employee);
                    }
                }
                return result;
            }
        }
            static void Main(string[] args)
            {
                CopperSmeltingControlSystem controlSystem = new CopperSmeltingControlSystem();
                CopperFurnace furnace = new CopperFurnace();
                CopperTemperatureSensor temperatureSensor = new CopperTemperatureSensor();

                controlSystem.TemperatureExceededThreshold += ControlSystem_TemperatureExceededThreshold;

                controlSystem.Start();
                furnace.MeltCopper();

                while (controlSystem.IsRunning)
                {
                    controlSystem.CheckTemperature(temperatureSensor, furnace.Temperature);
                    System.Threading.Thread.Sleep(1000);
                }

                furnace.StopMelting();
                controlSystem.Stop();

            EmployeeManagementSystem system = new EmployeeManagementSystem();

            
            system.AddEmployee(new Employee { Name = "John Doe", Position = "Manager", Salary = 5000 });
            system.AddEmployee(new Employee { Name = "Jane Smith", Position = "Developer", Salary = 4000 });
            system.AddEmployee(new Employee { Name = "Bob Johnson", Position = "Accountant", Salary = 3500 });

            
            Console.WriteLine("All Employees:");
            system.PrintAllEmployees();
            Console.WriteLine();

            
            double totalSalary = system.CalculateTotalSalary();
            Console.WriteLine("Total Salary: {0}", totalSalary);
            Console.WriteLine();

            
            double averageSalary = system.CalculateAverageSalary();
            Console.WriteLine("Average Salary: {0}", averageSalary);
            Console.WriteLine();

            
            double minSalary = 4000;
            List<Employee> highSalaryEmployees = system.FindEmployeesWithHigherSalary(minSalary);
            Console.WriteLine("Employees with Salary > {0}:", minSalary);
            foreach (var employee in highSalaryEmployees)
            {
                Console.WriteLine("Name: {0}, Position: {1}, Salary: {2}", employee.Name, employee.Position, employee.Salary);
            }
            Console.WriteLine();

            
            string employeeName = "Jane Smith";
            system.RemoveEmployee(employeeName);
            Console.WriteLine("Employee {0} has been removed.", employeeName);
            Console.WriteLine();

            
            Console.WriteLine("All Employees:");
            system.PrintAllEmployees();
        }
        }
    }