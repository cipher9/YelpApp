-- Function to calculate distance from user to business
CREATE OR REPLACE FUNCTION myDistance(
                            alat double precision,
                            alng double precision,
                            blat double precision,
                            blng double precision)
  RETURNS double precision
AS $$
      SELECT 3963.0 * acos
      (
        (
          sin(alat / (180 / pi())) * 
          sin(blat / (180 / pi()))) + 
          cos(alat / (180 / pi())) * 
          cos(blat / (180 / pi())) * 
          cos((blng / (180 / pi())) – (alng / (180 / pi())))
        ) AS dist;
$$  
LANGUAGE SQL;