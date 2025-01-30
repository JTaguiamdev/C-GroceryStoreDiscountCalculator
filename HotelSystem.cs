// Change the money value to PESOS.
// GUI

using System;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
    // User class
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // e.g., Admin, FrontDesk, Housekeeping, Guest
    }

    // Room class
    public class Room
    {
        public int RoomNumber { get; set; }
        public string Type { get; set; } // e.g., "Standard", "Deluxe", "Suite"
        public string Status { get; set; } // e.g., "Vacant", "Occupied", "UnderMaintenance"
        public decimal PricePerNight { get; set; }
    }

    // Guest class
    public class Guest
    {
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }

    // Booking class
    public class Booking
    {
        public int BookingId { get; set; }
        public int RoomNumber { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalCost { get; set; }
    }

    // Hotel class to manage rooms, guests, and bookings
    public class Hotel
    {
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Guest> Guests { get; set; } = new List<Guest>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<User> Users { get; set; } = new List<User>();

        // Initialize default rooms
        public void InitializeRooms()
        {
            for (int i = 1; i <= 500; i++)
            {
                string roomType = "Standard"; // Default type for all rooms
                decimal pricePerNight = 100; // Default price for all rooms

                // Customize room types and prices based on room number
                if (i % 100 == 0) // Every 100th room is Deluxe
                {
                    roomType = "Deluxe";
                    pricePerNight = 150;
                }
                else if (i % 50 == 0) // Every 50th room is Suite
                {
                    roomType = "Suite";
                    pricePerNight = 200;
                }

                Rooms.Add(new Room
                {
                    RoomNumber = i,
                    Type = roomType,
                    Status = "Vacant",
                    PricePerNight = pricePerNight
                });
            }
        }

        // Add a new user
        public void AddUser(string username, string password, string role)
        {
            Users.Add(new User
            {
                Id = Users.Count + 1,
                Username = username,
                Password = password,
                Role = role
            });
            Console.WriteLine($"User  {username} added successfully!");
        }

        // Book a room
        public void BookRoom(string guestName, string contactInfo, int roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            var room = Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber && r.Status == "Vacant");
            if (room == null)
            {
                Console.WriteLine("Room is not available.");
                return;
            }

            var guest = Guests.FirstOrDefault(g => g.Name == guestName);
            if (guest == null)
            {
                guest = new Guest { Name = guestName, ContactInfo = contactInfo };
                Guests.Add(guest);
            }

            int bookingId = Bookings.Count + 1;
            int nights = (checkOutDate - checkInDate).Days;
            decimal totalCost = room.PricePerNight * nights;

            var booking = new Booking
            {
                BookingId = bookingId,
                RoomNumber = roomNumber,
                GuestName = guestName,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                TotalCost = totalCost
            };

            Bookings.Add(booking);
            guest.Bookings.Add(booking);
            room.Status = "Occupied";

            Console.WriteLine($"Booking successful ! Booking ID: {bookingId}, Total Cost: {totalCost}");
        }

        // Check-out a guest
        public void CheckOut(int roomNumber)
        {
            var room = Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber && r.Status == "Occupied");
            if (room == null)
            {
                Console.WriteLine("Room is not occupied.");
                return;
            }

            var booking = Bookings.FirstOrDefault(b => b.RoomNumber == roomNumber && b.CheckOutDate >= DateTime.Now);
            if (booking != null)
            {
                Console.WriteLine($"Guest {booking.GuestName} checked out. Total bill: {booking.TotalCost}");
                room.Status = "Vacant";
            }
            else
            {
                Console.WriteLine("No active booking found for this room.");
            }
        }

        // Display all rooms
        public void DisplayRooms()
        {
            Console.WriteLine("Room List:");
            foreach (var room in Rooms)
            {
                Console.WriteLine($"Room {room.RoomNumber} - {room.Type} - {room.Status} - ${room.PricePerNight}/night");
            }
        }

        // Display all bookings
        public void DisplayBookings()
        {
            Console.WriteLine("Booking List:");
            foreach (var booking in Bookings)
            {
                Console.WriteLine($"Booking ID: {booking.BookingId}, Room: {booking.RoomNumber}, Guest: {booking.GuestName}, Check-In: {booking.CheckInDate.ToShortDateString()}, Check-Out: {booking.CheckOutDate.ToShortDateString()}, Total Cost: {booking.TotalCost}");
            }
        }

        // User login
        public User Login(string username, string password)
        {
            return Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // User registration
        public bool Register(string username, string password, string role)
        {
            if (Users.Any(u => u.Username == username))
            {
                Console.WriteLine("Username already exists. Please choose a different username.");
                return false;
            }

            AddUser(username, password, role);
            return true;
        }
    }

    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            Hotel hotel = new Hotel();
            hotel.InitializeRooms(); // Initialize default rooms

            while (true)
            {
                Console.WriteLine("Welcome to the Hotel Management System");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                string mainChoice = Console.ReadLine();

                if (mainChoice == "1")
                {
                    Console.Clear();
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    Console.Write("Enter role (Admin, FrontDesk, Housekeeping, Guest): ");
                    string role = Console.ReadLine();
                    Console.Clear();
                    hotel.Register(username, password, role);
                }
                else if (mainChoice == "2")
                {
                    Console.Clear();
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();

                    User loggedInUser = hotel.Login(username, password);
                    if (loggedInUser == null)
                    {
                        Console.WriteLine("Invalid username or password. Please try again.");
                        continue;
                    }

                    while (true)
                    {
                        Console.WriteLine($"\nWelcome {loggedInUser.Username} ({loggedInUser.Role})");
                        Console.WriteLine("1. Display Rooms");
                        Console.WriteLine("2. Book a Room");
                        Console.WriteLine("3. Check-Out");
                        Console.WriteLine("4. Display Bookings");
                        Console.WriteLine("5. Logout");
                        Console.Write("Enter your choice: ");
                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                hotel.DisplayRooms();
                                break;

                            case "2":
                                Console.Clear();
                                if (loggedInUser.Role == "FrontDesk" || loggedInUser.Role == "Admin")
                                {
                                    Console.Write("Enter guest name: ");
                                    string guestName = Console.ReadLine();
                                    Console.Write("Enter contact info: ");
                                    string contactInfo = Console.ReadLine();
                                    Console.Write("Enter room number: ");
                                    int roomNumber = int.Parse(Console.ReadLine());
                                    Console.Write("Enter check-in date (yyyy-mm-dd): ");
                                    DateTime checkInDate = DateTime.Parse(Console.ReadLine());
                                    Console.Write("Enter check-out date (yyyy-mm-dd): ");
                                    DateTime checkOutDate = DateTime.Parse(Console.ReadLine());
                                    hotel.BookRoom(guestName, contactInfo, roomNumber, checkInDate, checkOutDate);
                                }
                                else
                                {
                                    Console.WriteLine("You do not have permission to book a room.");
                                }
                                break;

                            case "3":
                                Console.Clear();
                                if (loggedInUser.Role == "FrontDesk" || loggedInUser.Role == "Admin")
                                {
                                    Console.Write("Enter room number to check-out: ");
                                    int checkOutRoomNumber = int.Parse(Console.ReadLine());
                                    hotel.CheckOut(checkOutRoomNumber);
                                }
                                else
                                {
                                    Console.WriteLine("You do not have permission to check-out a room.");
                                }
                                break;

                            case "4":
                                Console.Clear();
                                if (loggedInUser.Role == "Admin" || loggedInUser.Role == "FrontDesk")
                                {
                                    hotel.DisplayBookings();
                                }
                                else
                                {
                                    Console.WriteLine("You do not have permission to view bookings.");
                                }
                                break;

                            case "5":
                                Console.Clear();
                                Console.WriteLine("Logging out...");
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }

                        if (choice == "5")
                        {
                            break;
                        }
                    }
                }
                else if (mainChoice == "3")
                {
                    Console.WriteLine("Exiting the system. Goodbye!");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }
    }
}   
