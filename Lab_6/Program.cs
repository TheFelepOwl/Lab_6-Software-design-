using System;
using System.Collections.Generic;
using System.Linq;

// Перерахувальний тип даних для сигналiв датчикiв
public enum SensorSignal
{
    Temperature,
    Humidity,
    Light
}

// Клас для представлення даних датчикiв
public class SensorData
{
    public SensorSignal SignalType { get; set; }
    public double Value { get; set; }

    public SensorData(SensorSignal signalType, double value)
    {
        SignalType = signalType;
        Value = value;
    }
}

// Клас для валiдацiї даних
public static class DataValidator
{
    public static void ValidateCoordinates(double lat, double lon)
    {
        if (lat < -90 || lat > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(lat), "Широта повинна бути мiж -90 та 90");
        }
        if (lon < -180 || lon > 180)
        {
            throw new ArgumentOutOfRangeException(nameof(lon), "Довгота повинна бути мiж -180 та 180");
        }
    }

    public static void ValidateSensorData(SensorSignal signalType, double value)
    {
        switch (signalType)
        {
            case SensorSignal.Temperature:
                if (value < -50 || value > 50)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Температура повинна бути мiж -50 та 50");
                }
                break;
            case SensorSignal.Humidity:
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Вологiсть повинна бути мiж 0 та 100");
                }
                break;
            case SensorSignal.Light:
                if (value < 0 || value > 100000)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Освiтленiсть повинна бути мiж 0 та 100000");
                }
                break;
            default:
                throw new ArgumentException("Невiдомий тип сигналу датчика");
        }
    }
}

// Клас для реєстрацiї даних датчикiв
public class SensorDataRegistry
{
    private List<SensorData> data = new List<SensorData>();

    public void RegisterData(SensorData sensorData)
    {
        DataValidator.ValidateSensorData(sensorData.SignalType, sensorData.Value);
        data.Add(sensorData);
        string unit = GetUnit(sensorData.SignalType);
        Console.WriteLine($"Данi зареєстрованi: {sensorData.SignalType} - {sensorData.Value} {unit}");
    }

    private string GetUnit(SensorSignal signalType)
    {
        return signalType switch
        {
            SensorSignal.Temperature => "°C",
            SensorSignal.Humidity => "%",
            SensorSignal.Light => "лк",
            _ => ""
        };
    }

    public bool HasDataForSignalType(SensorSignal signalType)
    {
        return data.Any(d => d.SignalType == signalType);
    }
}

// Основна функцiя для демонстрацiї роботи з реєстрацiєю даних датчикiв
public class Program
{
    public static void Main(string[] args)
    {
        SensorDataRegistry registry = new SensorDataRegistry();

        // Приклад реєстрацiї даних вiд датчикiв
        try
        {
            SensorData temperatureData = new SensorData(SensorSignal.Temperature, 25);
            SensorData humidityData = new SensorData(SensorSignal.Humidity, 70);
            SensorData lightData = new SensorData(SensorSignal.Light, 50000);

            registry.RegisterData(temperatureData);
            registry.RegisterData(humidityData);
            registry.RegisterData(lightData);

            // Перевiрка наявностi даних для кожного сигналу
            foreach (SensorSignal signal in Enum.GetValues(typeof(SensorSignal)))
            {
                if (registry.HasDataForSignalType(signal))
                {
                    Console.WriteLine($"iснують данi для сигналу: {signal}");
                }
                else
                {
                    Console.WriteLine($"Немає даних для сигналу: {signal}");
                }
            }

            // Введення даних вiд користувача для кожного типу сигналу
            foreach (SensorSignal signal in Enum.GetValues(typeof(SensorSignal)))
            {
                Console.Write($"Введiть значення для {signal}: ");
                double userInput = Convert.ToDouble(Console.ReadLine());
                SensorData userSensorData = new SensorData(signal, userInput);
                registry.RegisterData(userSensorData);
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка валiдацiї: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
    }
}
