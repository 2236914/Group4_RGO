# PROJECT DOCUMENTATION
**Resources Generation Office System**

By: Group 4 - BSIT 2101
Arquillo, Sheena Mae
Angeles, April Haizel
Banares, Faith Anne

----------------------------------------
### I. Hypothesis
___-Problem Statement___
In the absence of a systematic data management solution, the Rural Green Organization (RGO) faced
challenges in organizing and retrieving crucial information. Recognizing this issue, our team, Group
4, embarked on a mission to develop a comprehensive database application system tailored to
address the specific needs of RGO.

___-Hypothesis___
The hypothesis underlying our project was that the lack of a dedicated system was impeding RGO's
efficiency in managing and utilizing their data resources. By creating a robust database application,
we aimed to provide RGO with a structured and efficient means of organizing their data, thereby
enhancing their overall operational effectiveness.

### II. Process
___-Team Collaboration___
Our team engaged in a collaborative effort to conceptualize, design, and implement the database
application system. The process involved numerous iterations and modifications as we navigated
through the complexities of software development. Challenges such as debugging and
troubleshooting were met with resilience, ensuring that the development process remained
dynamic and responsive

___-Development Adventures___
The journey of creating the application was marked by a series of modifications and adjustments.
The development phase presented us with unforeseen challenges, testing our problem-solving
skills and adaptability. Despite the inevitable stress associated with intricate problem-solving, the
team persevered, drawing satisfaction from the incremental progress made at each stage of
development.


___-Data Retrieval Success___
The pinnacle of our efforts was reached when the application proved successful in retrieving and
organizing data seamlessly. The system was meticulously designed to ensure accurate and efficient
data retrieval, contributing to the overall success of the project.


### III.  Query Used
These queries are designed to retrieve specific information from the database "rgo_lipa":

1. *ADMIN_PORTAL - List of Orders:*
- This query fetches details of orders placed by joining the students, items, and typeofuniform tables
based on matching sr_code and order_items_id. It displays the student details along with order-specific
information like date, types of items ordered, and uniform details.
2. *FEEDBACKS:*
- Retrieves all entries from the Feedback table sorted in ascending order based on SR_CODE.
3. *ITEMS:*
- Fetches all records from the Items table sorted in ascending order based on SR_CODE.
4. *PAYMENTS:*
- Retrieves all payment records from the Payment table sorted in ascending order based on SR_CODE.
5. *STAFFS:*
- Retrieves all staff details from the Staff table sorted in ascending order based on STAFF_ID.
6. *STUDENT_List of Orders:*
- Allows students to view their orders by fetching details from the students, items, and typeofuniform
tables. It shows the orders made by each student along with specific order details.
7. *STUDENTS - list of students who ordered:*
- Retrieves all student records from the students table sorted in ascending order based on SR_CODE.
8. *UNIFORM:*
- Fetches all records from the TypeOfUniform table sorted in ascending order based on
ORDER_UNIFORM_ID.


### V. Buttons Code

1. Items Table Operations:
- *Insert:*
- Inserts a new record into the Items table with specified attributes like ORDER_ITEMS_ID,
SR_CODE, DATE_OF_ORDER, UNIFORM_PINS, ID_LACE, and PAYMENT_ID.
- *Select:*
- Fetches data from an unspecified table where a specific column matches a given value.
- *Update:*
- Modifies an existing record in the Items table based on the ORDER_ITEMS_ID.
2. Payment Table Operations:
- *Insert:*
- Inserts a new record into the Payment table with attributes like PAYMENT_ID, SR_CODE,
DATE_OF_ORDER, AMOUNT, and STAFF_ID.
- *Select:*
- Similar to the previous case, it fetches data from an unspecified table based on a specific column's
value.
- *Update:*
- Updates an existing record in the Payment table based on the PAYMENT_ID.
3. Staff Table Operations:
- *Insert:*
- Adds a new record to the Staff table with attributes such as STAFF_ID, STAFF_NAME,
STAFF_POSITION, EMAIL, and PHONE.
- *Update:*
- Modifies an existing staff record in the Staff table based on the STAFF_ID.
- *Delete:*
- Deletes a staff record from the Staff table based on the STAFF_ID.
- *Select:*
- Fetches all records from the Staff table.
4. Students Table Operations:
- *Insert:*
- Inserts a new record into the Students table with attributes like SR_CODE,
COLLEGE_DEPARTMENT, STUDENT_NAME, EMAIL, PHONE, and DATE_OF_ORDER.
- *Update:*
- Modifies an existing student record in the Students table based on SR_CODE and
DATE_OF_ORDER.
- *Delete:*
- Deletes a student record from the Students table based on SR_CODE and DATE_OF_ORDER.
- *Select:*
- Fetches all records from the Students table.
- *Delete from Items:*
- Deletes records from the Items table based on SR_CODE and DATE_OF_ORDER.
5. TypeOfUniform Table Operations:
- *Insert:*
- Inserts a new record into the TypeOfUniform table with attributes like ORDER_UNIFORM_ID,
SR_CODE, DATE_OF_ORDER, WHITE_FABRIC, CHECKERED, PANTS, PE, and PAYMENT_ID.
- *Update:*
- Updates an existing record in the TypeOfUniform table based on ORDER_UNIFORM_ID.
The provided SQL commands cover a range of functionalities, including insertion, deletion,
updating, and retrieval of data from different tables within the database. These operations can be
used for managing staff, students, payments, items, and uniform-related records in the database.

### VI. Result
___-Project Completion___
The culmination of our collective endeavors resulted in the successful creation of the RGO Lipa
database application. The application is now fully operational, providing RGO with a powerful tool
to manage their data efficiently.

Achievements
• App Development: Successfully developed a comprehensive database application tailored
to RGO's requirements.
• Data Retrieval: Implemented a robust system for organizing and retrieving data effectively.
• Operational Impact: The completed application is poised to make a positive impact on
RGO's operations by streamlining their data management processes.

___-Conclusion___
In conclusion, our team's commitment to addressing RGO's data management challenges has
resulted in the successful creation of a functional and efficient database application. This endeavor
not only showcases our technical skills as second-year college students but also emphasizes our
dedication to real-world problem-solving and collaboration. We believe that the implemented
solution will significantly contribute to RGO's organizational goals and pave the way for future
enhancements in data management practices.

### VII. Flowchart and ERD
[Flowchart](https://drive.google.com/file/d/1jzfBrCtnLlm6DJeCTgt00sZ7vTAsYsuJ/view?usp=sharing)
[ERD](https://drive.google.com/file/d/1t0M9dmYSzVfEogpwSJdRJbrU_1lZPcgt/view?usp=sharing)

