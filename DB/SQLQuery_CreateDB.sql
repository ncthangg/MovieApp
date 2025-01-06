CREATE DATABASE MovieAppDB
USE MovieAppDB

-- Bảng User_Status
CREATE TABLE "User_Status" (
    "statusId" BIGINT IDENTITY(1,1)  PRIMARY KEY,
    "statusName" NVARCHAR(255) NOT NULL,
);

-- Bảng User_Role
CREATE TABLE "User_Role" (
    "roleId" BIGINT IDENTITY(1,1)  PRIMARY KEY,
    "roleName" NVARCHAR(255) NOT NULL,
);

-- Bảng User
CREATE TABLE "User" (
    "userId" BIGINT IDENTITY(1,1)  PRIMARY KEY,
    "name" NVARCHAR(255) NOT NULL,
    "email" NVARCHAR(255) NOT NULL,
    "passwordhash" NVARCHAR(MAX) NOT NULL,
    "roleId" BIGINT NOT NULL,
    "statusId" BIGINT NOT NULL,
	"isVerified" BIT NOT NULL DEFAULT 0,
    "createdAt" DATETIME NOT NULL,
    "updatedAt" DATETIME NOT NULL,
    CONSTRAINT "user_roleid_foreign" FOREIGN KEY ("roleId") REFERENCES "User_Role"("roleId"),
    CONSTRAINT "user_statusid_foreign" FOREIGN KEY ("statusId") REFERENCES "User_Status"("statusId")
);

CREATE UNIQUE INDEX "user_email_unique" ON "User"("email");

-- Bảng Category
CREATE TABLE "Category" (
    "categoryId" BIGINT IDENTITY(1,1)  PRIMARY KEY,
    "categoryName" NVARCHAR(255) NOT NULL,
);

-- Bảng Actor
CREATE TABLE "Actor" (
    "actorId" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "actorName" NVARCHAR(255) NOT NULL,
    "birthDate" DATE NULL,
    "bio" NVARCHAR(MAX) NULL
);

-- Bảng MovieType
CREATE TABLE "Movie_Type" (
    "typeId" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "typeName" NVARCHAR(255) NOT NULL
);

-- Bảng Movie
CREATE TABLE "Movie" (
    "movieId" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "movieName" NVARCHAR(255) NOT NULL,
    "description" NVARCHAR(MAX) NOT NULL,
    "videoUrl" NVARCHAR(MAX) NULL,
    "posterUrl" NVARCHAR(MAX) NULL,
    "director" NVARCHAR(255) NOT NULL,
    "releaseYear" INT NOT NULL,
	"typeId" BIGINT NOT NULL,
    "createdAt" DATETIME NULL,
    "updatedAt" DATETIME NULL,
	"isDeleted" BIT NOT NULL DEFAULT 0,
	CONSTRAINT "movie_type_typeid_foreign" FOREIGN KEY ("typeId") REFERENCES "Movie_Type"("typeId")
);

-- Bảng Movie_Actor
CREATE TABLE "Movie_Actor" (
    "id" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "movieId" BIGINT NOT NULL,
    "actorId" BIGINT NOT NULL,
    CONSTRAINT "movie_actor_movieid_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId"),
    CONSTRAINT "movie_actor_actorid_foreign" FOREIGN KEY ("actorId") REFERENCES "Actor"("actorId")
);

-- Bảng Season
CREATE TABLE "Movie_Season" (
    "seasonId" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "movieId" BIGINT NOT NULL,
    "seasonName" NVARCHAR(MAX) NOT NULL,
	"posterUrl" NVARCHAR(MAX) NULL,
    "releaseYear" INT NOT NULL,
    "createdAt" DATETIME NULL,
    "updatedAt" DATETIME NULL,
	"isDeleted" BIT NOT NULL DEFAULT 0,
	CONSTRAINT "movie_season_movie_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId")
ON DELETE CASCADE,
);

-- Bảng Episodes
CREATE TABLE "Movie_Episode" (
    "episodeId" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "seasonId" BIGINT NOT NULL,
    "episodeName" NVARCHAR(MAX) NOT NULL,
	"description" NVARCHAR(MAX) NOT NULL,
    "videoUrl" NVARCHAR(MAX) NULL,
    "posterUrl" NVARCHAR(MAX) NULL,
    "createdAt" DATETIME NULL,
    "updatedAt" DATETIME NULL,
	"isDeleted" BIT NOT NULL DEFAULT 0,
	CONSTRAINT "movie_episode_movie_season_foreign" FOREIGN KEY ("seasonId") REFERENCES "Movie_Season"("seasonId")
ON DELETE CASCADE,
);

-- Bảng Movie_Category
CREATE TABLE "Movie_Category" (
    "id" BIGINT IDENTITY(1,1) PRIMARY KEY ,
    "movieId" BIGINT NOT NULL,
    "categoryId" BIGINT NOT NULL,
    CONSTRAINT "movie_category_movieid_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId"),
    CONSTRAINT "movie_category_categoryid_foreign" FOREIGN KEY ("categoryId") REFERENCES "Category"("categoryId")
);

-- Bảng Movie_Rate
CREATE TABLE "Movie_Rate" (
    "id" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "movieId" BIGINT NOT NULL,
    "userId" BIGINT NOT NULL,
    "vote" BIGINT CHECK ("vote" BETWEEN 1 AND 5),
    "comment" NVARCHAR(MAX) NULL,
	"createdAt" DATETIME NULL,
    "updatedAt" DATETIME NULL,
    CONSTRAINT "movie_rate_movieid_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId"),
    CONSTRAINT "movie_rate_userid_foreign" FOREIGN KEY ("userId") REFERENCES "User"("userId")
);

-- Bảng User_WatchHistory
CREATE TABLE "User_WatchHistory" (
    "id" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "userId" BIGINT NOT NULL,
    "movieId" BIGINT NULL,
	"seasonId" BIGINT NULL,
	"episodeId" BIGINT NULL,
    "lastWatch" DATETIME NULL,
	"timeWatch" BIGINT NULL,
    CONSTRAINT "user_watchhistory_userid_foreign" FOREIGN KEY ("userId") REFERENCES "User"("userId"),
    CONSTRAINT "user_watchhistory_movieid_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId"),
	CONSTRAINT "user_watchhistory_seasonId_foreign" FOREIGN KEY ("seasonId") REFERENCES "Movie_Season"("seasonId"),
	CONSTRAINT "user_watchhistory_episodeId_foreign" FOREIGN KEY ("episodeId") REFERENCES "Movie_Episode"("episodeId")
);

-- Bảng User_Like
CREATE TABLE "User_Like" (
    "id" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "userId" BIGINT NOT NULL,
    "movieId" BIGINT NULL,
    "createdAt" DATETIME NULL,
    CONSTRAINT "user_like_userid_foreign" FOREIGN KEY ("userId") REFERENCES "User"("userId"),
    CONSTRAINT "user_like_movieid_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId")
);

-- Bảng User_Verification
CREATE TABLE "User_Verification" (
    "id" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "userId" BIGINT NOT NULL,
    "verificationCode" NVARCHAR(512) NOT NULL, -- Mã xác thực
    "expiresAt" DATETIME NOT NULL, -- Thời gian hết hạn
    "isUsed" BIT NOT NULL DEFAULT 0, -- Trạng thái đã sử dụng (0: chưa sử dụng, 1: đã sử dụng)
    "createdAt" DATETIME NOT NULL, -- Thời gian tạo
    CONSTRAINT "user_verification_userid_foreign" FOREIGN KEY ("userId") REFERENCES "User"("userId")
);

-- Bảng User_Token
CREATE TABLE "User_Token" (
    "tokenId" BIGINT IDENTITY(1,1) PRIMARY KEY,
    "userId" BIGINT UNIQUE,
    "refreshToken" NVARCHAR(512) NULL UNIQUE,
    "refreshToken_Expires" DATETIME NULL,
    "lastLogin" DATETIME NULL,
    CONSTRAINT "user_token_userid_foreign" FOREIGN KEY ("userId") REFERENCES "User"("userId") ON DELETE CASCADE
);

drop table Actor
drop table Category
drop table Movie_Category
drop table Movie
drop table Movie_Type
drop table Movie_Season
drop table Movie_Episode
drop table Movie_Rate
drop table [User]
drop table User_Like
drop table User_Role
drop table User_Status
drop table User_Token
drop table User_WatchHistory