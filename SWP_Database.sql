-- Create User table
CREATE TABLE [User] (
    id INT PRIMARY KEY IDENTITY(1,1),
    email NVARCHAR(255) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    fullName NVARCHAR(255) NOT NULL,
    phone NVARCHAR(10), -- Phone number contains 10 digits
    gender NVARCHAR(10),
    profilePhoto NVARCHAR(255),
    role NVARCHAR(50) NOT NULL DEFAULT 'default' CHECK (role IN ('admin', 'student', 'mentor', 'default')),
    isFirstLogin BIT NOT NULL DEFAULT 1,
    isActive BIT NOT NULL DEFAULT 1
);

-- Modify MentorDetail table
CREATE TABLE MentorDetail (
    userId INT PRIMARY KEY,
    mainProgrammingLanguage NVARCHAR(50),
    altProgrammingLanguage NVARCHAR(255),
    framework NVARCHAR(50),
    education NVARCHAR(255),
    additionalContactInfo NVARCHAR(255),
    bookingScore INT DEFAULT 0,
    description NVARCHAR(MAX),
    FOREIGN KEY (userId) REFERENCES [User](id)
);

-- Modify StudentDetail table
CREATE TABLE StudentDetail (
    userId INT PRIMARY KEY,
    studentCode NVARCHAR(8) NOT NULL, -- Format: (2 letters) + (6 digits)
    groupId INT,
    isLeader BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (userId) REFERENCES [User](id)
);

-- Create UserSession table
CREATE TABLE UserSession (
    id INT PRIMARY KEY IDENTITY(1,1),
    userId INT NOT NULL,
    expireTime DATETIME NOT NULL,
    FOREIGN KEY (userId) REFERENCES [User](id)
);

-- Create UserActivityType table
CREATE TABLE UserActivityType (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(255),
);

-- Create UserActivity table
CREATE TABLE UserActivity (
    id INT PRIMARY KEY IDENTITY(1,1),
    activityTypeId INT NOT NULL,
    description NVARCHAR(255),
    operatorId INT NOT NULL,
    createdAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (activityTypeId) REFERENCES UserActivityType(id),
    FOREIGN KEY (operatorId) REFERENCES [User](id)
);

-- Create Topic table
CREATE TABLE Topic (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL UNIQUE,
    description NVARCHAR(255)
);

-- Create Specialization table
CREATE TABLE Specialization (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(255)
);

-- Create MentorSpecialization table
CREATE TABLE MentorSpecialization (
    mentorDetailId INT NOT NULL,
    specId INT NOT NULL,
    PRIMARY KEY (mentorDetailId, specId),
    FOREIGN KEY (mentorDetailId) REFERENCES MentorDetail(userId),
    FOREIGN KEY (specId) REFERENCES Specialization(id)
);

-- Create Wallet table
CREATE TABLE Wallet (
    id INT PRIMARY KEY IDENTITY(1,1),
    balance INT NOT NULL DEFAULT 0
);

-- Modify StudentGroup table
CREATE TABLE StudentGroup (
    id INT PRIMARY KEY IDENTITY(1,1),
    groupName NVARCHAR(100) NOT NULL UNIQUE,
    topicId INT NOT NULL,
    walletId INT NOT NULL UNIQUE,
    FOREIGN KEY (topicId) REFERENCES Topic(id),
    FOREIGN KEY (walletId) REFERENCES Wallet(id)
);

-- Update StudentDetail table to add FOREIGN KEY for groupId
ALTER TABLE StudentDetail
ADD FOREIGN KEY (groupId) REFERENCES StudentGroup(id);

-- Create Skill table
CREATE TABLE Skill (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(255)
);

-- Create WalletTransaction table
CREATE TABLE WalletTransaction (
    id INT PRIMARY KEY IDENTITY(1,1),
    walletId INT NOT NULL,
    amount INT NOT NULL,
    type NVARCHAR(20) NOT NULL CHECK (type IN ('addition', 'deduction')),
    date DATETIME NOT NULL DEFAULT GETDATE(),
    description NVARCHAR(255),
    FOREIGN KEY (walletId) REFERENCES Wallet(id)
);

-- Modify AdminDetail table
CREATE TABLE AdminDetail (
    userId INT PRIMARY KEY,
    FOREIGN KEY (userId) REFERENCES [User](id)
);

-- Create Slot table
CREATE TABLE Slot (
    id INT PRIMARY KEY IDENTITY(1,1),
    startTime TIME NOT NULL, -- Format: HH:MM
    endTime TIME NOT NULL -- Format: HH:MM
);

-- Create MentorSchedule table
CREATE TABLE MentorSchedule (
    id INT PRIMARY KEY IDENTITY(1,1),
    mentorDetailId INT NOT NULL,
    slotId INT NOT NULL,
    date DATE NOT NULL,
    status NVARCHAR(20) NOT NULL DEFAULT 'available' CHECK (status IN ('available', 'booked', 'unavailable')),
    FOREIGN KEY (mentorDetailId) REFERENCES MentorDetail(userId),
    FOREIGN KEY (slotId) REFERENCES Slot(id)
);

-- Create Booking table
CREATE TABLE Booking (
    id INT PRIMARY KEY IDENTITY(1,1),
    leaderId INT NOT NULL,
    mentorScheduleId INT NOT NULL,
    timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    note NVARCHAR(255),
    FOREIGN KEY (leaderId) REFERENCES StudentDetail(userId),
    FOREIGN KEY (mentorScheduleId) REFERENCES MentorSchedule(id)
);

-- Create Feedback table
CREATE TABLE Feedback (
    id INT PRIMARY KEY IDENTITY(1,1),
    bookingId INT NOT NULL,
    givenBy INT NOT NULL,
    givenTo INT NOT NULL,
    rating INT NOT NULL CHECK (rating BETWEEN 1 AND 5),
    comment NVARCHAR(MAX),
    date DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (bookingId) REFERENCES Booking(id),
    FOREIGN KEY (givenBy) REFERENCES [User](id),
    FOREIGN KEY (givenTo) REFERENCES [User](id)
);

-- Modify Request table
CREATE TABLE Request (
    id INT PRIMARY KEY IDENTITY(1,1),
    leaderId INT NOT NULL,
    title NVARCHAR(255) NOT NULL,
    content NVARCHAR(MAX) NOT NULL,
    timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    status NVARCHAR(20) NOT NULL DEFAULT 'pending' CHECK (status IN ('pending', 'in process', 'approved', 'rejected')),
    FOREIGN KEY (leaderId) REFERENCES StudentDetail(userId)
);

-- Create Response table
CREATE TABLE Response (
    id INT PRIMARY KEY IDENTITY(1,1),
    requestId INT NOT NULL,
    content NVARCHAR(MAX) NOT NULL,
    timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (requestId) REFERENCES Request(id)
);

-- Create MentorSkill table
CREATE TABLE MentorSkill (
    mentorDetailId INT NOT NULL,
    skillId INT NOT NULL,
    PRIMARY KEY (mentorDetailId, skillId),
    FOREIGN KEY (mentorDetailId) REFERENCES MentorDetail(userId),
    FOREIGN KEY (skillId) REFERENCES Skill(id)
);

ALTER TABLE StudentDetail
ADD CONSTRAINT FK_StudentDetail_StudentGroup
FOREIGN KEY (groupId) REFERENCES StudentGroup(id);


-- Insert sample users with isFirstLogin set to true
Insert Into [dbo].[User] ([email],[password],[fullName],[phone],[gender],[profilePhoto],[role],[isFirstLogin],[isActive])
 Values('maihainam8@gmail.com', 'Password123!', 'Admin User', '1234567890', 'Male', 'admin_photo.jpg', 'admin', 1, 1);