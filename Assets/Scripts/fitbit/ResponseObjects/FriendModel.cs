using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResponseObjects
{
    public class FriendModel
    {
        public string aboutMe, avatar, city, country, dateOfBirth, displayName, encodedId,
            fullName, gender, nickname, state, timezone;
        public double height, offsetFromUTCMillis, strideLengthRunning, strideLengthWalking, weight;
        JSONObject jsonObject;

        public FriendModel(JSONObject jsonObject)
        {
            this.jsonObject = jsonObject;
            //TODO extract more info here if we want
            jsonObject.GetField("aboutMe", delegate(JSONObject aboutMe)
            {
                this.aboutMe = aboutMe.ToString().Substring(1, aboutMe.ToString().Length - 2);
            });
            jsonObject.GetField("avatar", delegate(JSONObject avatar)
            {
                this.avatar = avatar.ToString().Substring(1,avatar.ToString().Length-2);
            });
            jsonObject.GetField("city", delegate(JSONObject city)
            {
                this.city = city.ToString().Substring(1, city.ToString().Length - 2);
            });
            jsonObject.GetField("country", delegate(JSONObject country)
            {
                this.country = country.ToString().Substring(1, country.ToString().Length - 2);
            });
            jsonObject.GetField("dateOfBirth", delegate(JSONObject dateOfBirth)
            {
                this.dateOfBirth = dateOfBirth.ToString().Substring(1, dateOfBirth.ToString().Length - 2);
            });
            jsonObject.GetField("displayName", delegate(JSONObject displayName)
            {
                this.displayName = displayName.ToString().Substring(1, displayName.ToString().Length - 2);
            });
            jsonObject.GetField("encodedId", delegate(JSONObject encodedId)
            {
                this.encodedId = encodedId.ToString().Substring(1, encodedId.ToString().Length - 2);
            });
            jsonObject.GetField("fullName", delegate(JSONObject fullName)
            {
                this.fullName = fullName.ToString().Substring(1, fullName.ToString().Length - 2);
            });
            jsonObject.GetField("gender", delegate(JSONObject gender)
            {
                this.gender = gender.ToString().Substring(1, gender.ToString().Length - 2);
            });
            jsonObject.GetField("nickname", delegate(JSONObject nickname)
            {
                this.nickname = nickname.ToString().Substring(1, nickname.ToString().Length - 2);
            });
            jsonObject.GetField("timezone", delegate(JSONObject timezone)
            {
                this.timezone = timezone.ToString().Substring(1, timezone.ToString().Length - 2);
            });
            jsonObject.GetField("gender", delegate(JSONObject gender)
            {
                this.gender = gender.ToString().Substring(1, gender.ToString().Length - 2);
            });
            jsonObject.GetField("height", delegate(JSONObject height)
            {
                this.height = Convert.ToDouble(height.ToString());
            });
            jsonObject.GetField("offsetFromUTCMillis", delegate(JSONObject offsetFromUTCMillis)
            {
                this.offsetFromUTCMillis = Convert.ToDouble(offsetFromUTCMillis.ToString());
            });
            jsonObject.GetField("strideLengthRunning", delegate(JSONObject strideLengthRunning)
            {
                this.strideLengthRunning = Convert.ToDouble(strideLengthRunning.ToString());
            });
            jsonObject.GetField("strideLengthWalking", delegate(JSONObject strideLengthWalking)
            {
                this.strideLengthWalking = Convert.ToDouble(strideLengthWalking.ToString());
            });
            jsonObject.GetField("weight", delegate(JSONObject weight)
            {
                this.weight = Convert.ToDouble(weight.ToString());
            });
        }

        public override string ToString()
        {
            return jsonObject.ToString();
        }

    }
}
