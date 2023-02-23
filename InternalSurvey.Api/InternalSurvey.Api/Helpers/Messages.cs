using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Helpers
{
    public static class Messages
    {
        public static string UNEXPECTED_ERROR = "An unexpected error has occured";
        public static string INCOMPLETE_DATA = "The request has incomplete data";
        public static string CANNOT_CREATE_ENTITY = "Cannot create {0}";
        public static string ENTITY_NOT_FOUND = "Not found";
        public static string TRANSACTION_FAILED = "Transaction failed";
        public static string NO_DATA_FOR_ID = "No data for id: {0}";
        public static string NO_DATA = "No data";
        public static string CANNOT_FETCH_USER = "Could not fetch user";
        public static string CANNOT_FETCH_USER_DATA = "Could not fetch user data";
        public static string INVALID_MODEL_STATE = "Invalid model state";
        public static string INVALID_PAGE = "Invalid page";
        public static string HAS_EXPIRED = "{0} has expired";
        public static string HAS_VOTED = "{0} has voted";
        public static string DELETED_SUCCESSFULLY = "The {0} was deleted successfully";
        public static string UPDATED_SUCCESSFULLY = "Updated successfully";
        public static string PUBLISHED_SUCCESSFULLY = "Published successfully";
        public static string CREATED_SUCCESSFULLY = "Created successfully";
        public static string NOT_FOUND = "{0} not found";
        public static string ACCESS_DENIED = "Access denied";
        public static string NOT_ALLOWED_OPERATION = "You are not allowed to perform this operation";
        public static string BODY_EMAIL_NEW_SURVEY = "<p>Hi, there!</p><p> Please share your opionion and your answers will allow us to better meet your expectations!</p><p> We thank you for devoting 1 minute of your time to our survey!</p> <p> Please <a href=\"{0}\" target=\"_blank\">  click this link </a>to fill the survey!</p>";
        public static string OBJECT_EMAIL_NEW_SURVEY = "New Survey {0}";
        public static string BODY_EMAIL_REMINDER_SURVEY = "<p>Hi, there!</p>" +
            "<p>This is a reminder! Please share your opionion and your answers will allow us to better meet your expectations!</p>" +
            " <p>We thank you for devoting 1 minute of your time to our survey! </p>" +
            "<p>Please <a href='{0}'>click this link </a> to fill the survey</p>" +
            "<p>If you are not interested in this survey,<a href='{1}'> click here</a>  to switch off the reminder.</p>";
        public static string OBJECT_EMAIL_REMINDER_SURVEY = "Reminder Survey {0}";
    }
}