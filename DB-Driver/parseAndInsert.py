import json
import sys
import psycopg2

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'

def insert2BusinessTable():
    #reading the JSON file
    with open('.\Yelp-JSON\yelp_business.JSON','r') as f:
        #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0
 
        while line:
            data = json.loads(line)

            # initialize checkins, reviewcount to 0, reviewrating to 0.0
            sql_str = "INSERT INTO Business (business_id, name, address, review_rating, city, state, postal_code, latitude, longitude, stars, review_count, num_checkins, is_open) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["address"]) + "', 0.0,'" + \
                      cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["state"]) + "','" + cleanStr4SQL(data["postal_code"]) + "','" + str(data["latitude"]) + "','" + \
                      str(data["longitude"]) + "','" + str(data["stars"]) + "','" + "0 '," + "'0 ','"  + \
                      str(data["is_open"]) + "');"
            try:
                cur.execute(sql_str)
            except Exception as error:
                print('ERROR: Insert to business table failed!')
                print('Error message:', error)

            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2yelpUserTable():
    with open('.\Yelp-JSON\yelp_user.JSON','r') as f:
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)
 
            sql_str = "INSERT INTO yelpUser (average_stars, useful, user_id, cool, name, funny, yelping_since) " \
                      "VALUES ('" + str(data["average_stars"]) + "','" + str(data["useful"]) + "','" + cleanStr4SQL(data["user_id"]) + "','" + str(data["cool"]) + "','" + \
                      cleanStr4SQL(data['name']) + "','" + str(data["funny"]) + "','" + cleanStr4SQL(data["yelping_since"]) + "');"
            #print(sql_str)
            try:
                cur.execute(sql_str)
            except Exception as error:
                print('ERROR: Insert to yelpUser table failed!')
                print('Error message:', error)
                sys.exit()

            conn.commit()

            line = f.readline()
            count_line +=1

    print(count_line)
    f.close()

def insert2CheckinTable():
    with open('.\Yelp-JSON\yelp_checkin.JSON','r') as f:
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)
 
            for day, hours in data['time'].items():
                for hour, count in hours.items():
                    sql_str = "INSERT INTO CheckIn (business_id, checkin_count, checkin_day, checkin_hour) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + str(count) + "','" + str(day) + "','" + str(hour) + "');"
                    #print(sql_str)
                    try:
                        cur.execute(sql_str)
                    except Exception as error:
                        print('ERROR: Insert to checkin table failed!')
                        print('Error message:', error)

                    conn.commit()
            

            line = f.readline()
            count_line +=1

    print(count_line)
    f.close()

def insert2ReviewTable():
    with open('.\Yelp-JSON\yelp_review.JSON','r') as f:
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)
 
            sql_str = "INSERT INTO Review (review_id, user_id, business_id, review_date, review_text, stars, useful, funny, cool) " \
                      "VALUES ('" + cleanStr4SQL(data['review_id']) + "','" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(data['business_id']) + "','" + \
                        cleanStr4SQL(data['date']) + "','" + cleanStr4SQL(data['text']) + "','" + str(data['stars']) + "','" + \
                        str(data['useful']) + "','" + str(data['funny']) + "','" + str(data['cool']) + "');"
            
            try:
                cur.execute(sql_str)
            except Exception as error:
                print('ERROR: Insert to review table failed!')
                print('Error message:', error)

            conn.commit()

            line = f.readline()
            count_line +=1

    print(count_line)
    f.close()

def insert2FriendshipTable():
    with open('.\Yelp-JSON\yelp_user.JSON','r') as f:
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)
 
            for friend in data['friends']:
                sql_str = "INSERT INTO Friendship (user_id, users_friend_id) " \
                        "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(friend) + "');"
                
                try:
                    cur.execute(sql_str)
                except Exception as error:
                    print('ERROR: Insert to Friendship table failed!')
                    print('Error message:', error)

            conn.commit()

            line = f.readline()
            count_line +=1

    print(count_line)
    f.close()

def insert2CategoriesTable():
    with open('.\Yelp-JSON\yelp_business.JSON','r') as f:
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)
 
            for category in data['categories']:
                sql_str = "INSERT INTO Categories (business_id, category_name) " \
                        "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(category) + "');"
                
                try:
                    cur.execute(sql_str)
                except Exception as error:
                    print('ERROR: Insert to Categories table failed!')
                    print('Error message:', error)

            conn.commit()

            line = f.readline()
            count_line +=1

    print(count_line)
    f.close()

def insert2HoursTable():
    with open('.\Yelp-JSON\yelp_business.JSON','r') as f:
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)
 
            for day, hours in data['hours'].items():
                hours_parts = hours.split('-')
                open_time = hours_parts[0]
                close_time = hours_parts[1]

                sql_str = "INSERT INTO Hours (business_id, day, open_time, close_time) " \
                        "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + day + "','" + open_time + "','" + close_time + "');"
                
                try:
                    cur.execute(sql_str)
                except Exception as error:
                    print('ERROR: Insert to Hours table failed!')
                    print('Error message:', error)

            conn.commit()

            line = f.readline()
            count_line +=1

    print(count_line)
    f.close()

# Change password as necessary
try:
    conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='none'")
except Exception as error:
    print('ERROR: Unable to connect to the database!')
    print('Error message:', error)
cur = conn.cursor()

insert2BusinessTable()
insert2yelpUserTable()
insert2CheckinTable()
insert2ReviewTable()
insert2FriendshipTable()
insert2CategoriesTable()
insert2HoursTable()

cur.close()
conn.close()