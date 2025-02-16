namespace CC_Model
{
    public static class Enums
    {
        public enum HttpMethods
        {
            GET,
            POST,
        }

        public enum IsActive
        {
            DELETED = -1,
            IN_ACTIVE = 0,
            ACTIVE = 1
        }
    }
}