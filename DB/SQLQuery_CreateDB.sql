CREATE DATABASE MovieAppDB
USE MovieAppDB

-- Bảng User_Status
CREATE TABLE "User_Status" (
    "statusId" BIGINT IDENTITY(1,1),
    "statusName" NVARCHAR(255) NOT NULL,
    CONSTRAINT "user_status_statusid_primary" PRIMARY KEY ("statusId")
);

-- Bảng User_Role
CREATE TABLE "User_Role" (
    "roleId" BIGINT IDENTITY(1,1),
    "roleName" NVARCHAR(255) NOT NULL,
    CONSTRAINT "user_role_roleid_primary" PRIMARY KEY ("roleId")
);

-- Bảng User
CREATE TABLE "User" (
    "userId" BIGINT IDENTITY(1,1),
    "name" NVARCHAR(255) NOT NULL,
    "email" NVARCHAR(255) NOT NULL,
    "passwordhash" NVARCHAR(MAX) NOT NULL,
    "roleId" BIGINT NOT NULL,
    "statusId" BIGINT NOT NULL,
	"isVerified" BIT NOT NULL DEFAULT 0,
    "createdAt" DATETIME NOT NULL,
    "updatedAt" DATETIME NOT NULL,
    CONSTRAINT "user_userid_primary" PRIMARY KEY ("userId"),
    CONSTRAINT "user_roleid_foreign" FOREIGN KEY ("roleId") REFERENCES "User_Role"("roleId"),
    CONSTRAINT "user_statusid_foreign" FOREIGN KEY ("statusId") REFERENCES "User_Status"("statusId")
);

CREATE UNIQUE INDEX "user_email_unique" ON "User"("email");

-- Bảng Category
CREATE TABLE "Category" (
    "categoryId" BIGINT IDENTITY(1,1),
    "categoryName" NVARCHAR(255) NOT NULL,
    CONSTRAINT "category_categoryid_primary" PRIMARY KEY ("categoryId")
);

-- Bảng Movie
CREATE TABLE "Movie" (
    "movieId" BIGINT IDENTITY(1,1),
    "movieName" NVARCHAR(255) NOT NULL,
    "description" NVARCHAR(MAX) NOT NULL,
    "videoUrl" NVARCHAR(MAX) NULL,
    "posterUrl" NVARCHAR(MAX) NULL,
    "director" NVARCHAR(255) NOT NULL,
    "releaseYear" INT NOT NULL,
    "createdAt" DATETIME NULL,
    "updatedAt" DATETIME NULL,
	"isDeleted" BIT NOT NULL DEFAULT 0,
    CONSTRAINT "movie_movieid_primary" PRIMARY KEY ("movieId")
);
-- Bảng Movie_Category
CREATE TABLE "Movie_Category" (
    "id" BIGINT IDENTITY(1,1),
    "movieId" BIGINT NOT NULL,
    "categoryId" BIGINT NOT NULL,
    CONSTRAINT "movie_category_id_primary" PRIMARY KEY ("id"),
    CONSTRAINT "movie_category_movieid_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId"),
    CONSTRAINT "movie_category_categoryid_foreign" FOREIGN KEY ("categoryId") REFERENCES "Category"("categoryId")
);

-- Bảng Movie_Rate
CREATE TABLE "Movie_Rate" (
    "id" BIGINT IDENTITY(1,1),
    "movieId" BIGINT NOT NULL,
    "userId" BIGINT NOT NULL,
    "vote" BIGINT CHECK ("vote" BETWEEN 1 AND 5),
    "comment" NVARCHAR(MAX) NULL,
	"createdAt" DATETIME NULL,
    "updatedAt" DATETIME NULL,
    CONSTRAINT "movie_rate_id_primary" PRIMARY KEY ("id"),
    CONSTRAINT "movie_rate_movieid_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId"),
    CONSTRAINT "movie_rate_userid_foreign" FOREIGN KEY ("userId") REFERENCES "User"("userId")
);

-- Bảng User_WatchHistory
CREATE TABLE "User_WatchHistory" (
    "id" BIGINT IDENTITY(1,1),
    "userId" BIGINT NOT NULL,
    "movieId" BIGINT NULL,
    "watchAt" DATETIME NULL,
    CONSTRAINT "user_watchhistory_id_primary" PRIMARY KEY ("id"),
    CONSTRAINT "user_watchhistory_userid_foreign" FOREIGN KEY ("userId") REFERENCES "User"("userId"),
    CONSTRAINT "user_watchhistory_movieid_foreign" FOREIGN KEY ("movieId") REFERENCES "Movie"("movieId")
);

-- Bảng User_Like
CREATE TABLE "User_Like" (
    "id" BIGINT IDENTITY(1,1),
    "userId" BIGINT NOT NULL,
    "movieId" BIGINT NULL,
    "createdAt" DATETIME NULL,
    CONSTRAINT "user_like_id_primary" PRIMARY KEY ("id"),
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
