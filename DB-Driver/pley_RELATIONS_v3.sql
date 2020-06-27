CREATE TABLE Business (
	business_id CHAR(22) NOT NULL,
	name VARCHAR,
	address VARCHAR,
	review_rating FLOAT,
	city VARCHAR,
	state CHAR(2),
	postal_code INTEGER,
	latitude FLOAT,
	longitude FLOAT,
	stars FLOAT,
	review_count INTEGER,
	num_checkins INTEGER,
	is_open BIT,	-- 1 or 0 represent t/f
	PRIMARY KEY (business_id)
);

CREATE TABLE Categories (	-- weak entity
	business_id CHAR(22),
	category_name VARCHAR NOT NULL,
	PRIMARY KEY (business_id, category_name),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE Hours (	-- weak entity
	business_id CHAR(22),
	day VARCHAR NOT NULL,
	open_time TIME,
	close_time TIME,
	PRIMARY KEY (business_id, day),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE CheckIn (	-- weak entity
	business_id CHAR(22),
	checkin_count INTEGER NOT NULL,
	checkin_hour TIME NOT NULL,
	checkin_day VARCHAR,
	PRIMARY KEY (business_id, checkin_day, checkin_hour),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE yelpUser(
	average_stars FLOAT,
	useful INTEGER,
	user_id CHAR(22) NOT NULL,
	cool INTEGER,
	name VARCHAR,
	funny INTEGER,
	yelping_since DATE,
    latitude FLOAT,
	longitude FLOAT,
	PRIMARY KEY(user_id)
);

CREATE TABLE Review(
    review_id CHAR(22),
    user_id CHAR(22) NOT NULL,
    business_id CHAR(22) NOT NULL,
    review_date DATE,
    review_text VARCHAR(5000),    -- yelps max review length is 5000 characters
    stars FLOAT,
    useful INTEGER,
    funny INTEGER,
    cool INTEGER,
    PRIMARY KEY (review_id),
    FOREIGN KEY (user_id) REFERENCES yelpUser(user_id),
    FOREIGN KEY (business_id) REFERENCES Business(business_id)
);


CREATE TABLE Friendship(
	user_id CHAR(22) NOT NULL,
	users_friend_id CHAR(22) NOT NULL,
	PRIMARY KEY (user_id, users_friend_id),
	FOREIGN KEY (user_id) REFERENCES yelpUser(user_id),
	FOREIGN KEY (users_friend_id) REFERENCES yelpUser(user_id)
);

CREATE TABLE Favorites(
    user_id CHAR(22) NOT NULL,
    business_id CHAR(22) NOT NULL,
    PRIMARY KEY (user_id, business_id),
    FOREIGN KEY (user_id) REFERENCES yelpUser(user_id),
    FOREIGN KEY (business_id) REFERENCES Business(business_id)
);