-- update num_checkins
-- Sum of all check-in counts for that business
WITH R AS (
	SELECT 
		Checkin.business_id,
		SUM(checkin_count)
	FROM CheckIn
	GROUP BY CheckIn.business_id
)
UPDATE Business AS B
SET num_checkins = (SELECT R.sum FROM R WHERE B.business_id = R.business_id);

-- Update review_count and review_rating
WITH R AS (
    SELECT 
        Review.business_id,
        COUNT(*) as numReviews,
        AVG(stars) as averageRating
    FROM Review
    GROUP BY Review.business_id
)
UPDATE Business AS B
SET 
	review_count = (SELECT R.numReviews FROM R WHERE B.business_id = R.business_id),
	review_rating = (SELECT R.averageRating FROM R WHERE B.business_id = R.business_id);

