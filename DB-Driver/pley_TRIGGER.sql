
-- 5(a) reviewCount and reviewRating should be updated
-- when a new review is provided for a business review_count is incremented
CREATE OR REPLACE FUNCTION UpdateReviewCount() RETURNS trigger AS '
BEGIN 
	UPDATE  Business
	SET  review_count= review_count + 1
	WHERE Business.business_id = NEW.business_id;
	RETURN NEW;
END
' LANGUAGE plpgsql;  
    
CREATE TRIGGER reviewCount
AFTER INSERT ON Review
FOR EACH ROW
WHEN (NEW.business_id IS NOT NULL)
EXECUTE PROCEDURE UpdateReviewCount();

-- to get the reviewRating the average of Review.stars is calculated and updated
CREATE OR REPLACE FUNCTION UpdateReviewRating() RETURNS trigger AS '
BEGIN 
   UPDATE Business
   SET review_rating = (SELECT average 
						FROM (SELECT Review.business_id, AVG(Review.stars) as average
								FROM Review
								WHERE Review.business_id = NEW.business_id
								GROUP BY Review.business_id) as average_rating)
	WHERE Business.business_id = NEW.business_id;
	RETURN NEW;
END
' LANGUAGE plpgsql; 

CREATE TRIGGER reviewRating
AFTER INSERT ON Review
FOR EACH ROW
EXECUTE PROCEDURE UpdateReviewRating();

/* 	Run the following sequence of SQL statements in psql to test test reviewCount and reviewRating triggers
	SELECT * FROM Business WHERE business_id = 'OQcvO5P3gH0cuJ-bPXwfQQ';
	INSERT INTO  Review VALUES ('ClgrKJ6dqiM7vSKJBJ2w69', 'T5MGS0NHBCWgofZ6Q6Btng', 'OQcvO5P3gH0cuJ-bPXwfQQ', '2020-02-22', 'The food here is awesome!', 3, 1, 0, 0);
	SELECT * FROM Business WHERE business_id = 'OQcvO5P3gH0cuJ-bPXwfQQ';
	DELETE FROM Review WHERE Review.review_id='ClgrKJ6dqiM7vSKJBJ2w69';
*/
-----------------------------------------------------------------------
-----------------------------------------------------------------------

-- 5(b) numCheckins for business should be updated when customer checks in
-- when there is a new checkin for a business the num_checkins is incremented
CREATE OR REPLACE FUNCTION UpdateCheckin() RETURNS trigger AS '
BEGIN
	UPDATE  Business
	SET  num_checkins = num_checkins + 1
	WHERE Business.business_id=NEW.business_id;
	RETURN NEW;
END
' LANGUAGE plpgsql;  
    

CREATE TRIGGER numCheckins
AFTER INSERT OR UPDATE ON CheckIn
FOR EACH ROW
WHEN (NEW.business_id IS NOT NULL)
EXECUTE PROCEDURE UpdateCheckin();


CREATE OR REPLACE FUNCTION InsertCheckIn() RETURNS trigger AS '
BEGIN
	INSERT INTO CheckIn VALUES 
	(NEW.business_id, 0, NEW.checkin_time, NEW.checkin_day);
	RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER checkCheckCheckin
BEFORE INSERT OR UPDATE ON CheckIn
FOR EACH ROW
WHEN (NEW.checkin_hour IS NULL AND NEW.checkin_hour IS NULL)
	EXECUTE PROCEDURE InsertCheckIn();

/* Run the following sequence of SQL statements in psql to test numCheckins trigger
	SELECT * FROM Business WHERE business_id = 'dwQEZBFen2GdihLLfWeexA';
	SELECT * FROM CheckIn WHERE business_id = 'dwQEZBFen2GdihLLfWeexA';
	INSERT INTO CheckIn VALUES ('dwQEZBFen2GdihLLfWeexA', 1, '19:00:00', 'Monday');
	UPDATE Business SET num_checkins = num_checkins - 1 WHERE business_id = 'dwQEZBFen2GdihLLfWeexA';
	SELECT * FROM Business WHERE business_id = 'dwQEZBFen2GdihLLfWeexA';
	SELECT * FROM CheckIn WHERE business_id = 'dwQEZBFen2GdihLLfWeexA';
*/