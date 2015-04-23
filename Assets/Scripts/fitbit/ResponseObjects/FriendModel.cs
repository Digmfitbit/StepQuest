using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResponseObjects
{
    class FriendModel
    {
        public string aboutMe, avatar, city, country, dateOfBirth, displayName, encodedId,
            fullName, gender, nickname, state, timezone;
        public double height, offsetFromUTCMillis, strideLengthRunning, strideLengthWalking, weight;

        public FriendModel(JSONObject jsonObject)
        {
            //TODO extract more info here if we want
            jsonObject.GetField("aboutMe", delegate(JSONObject aboutMe)
            {
                this.aboutMe = aboutMe.ToString();
            });
            jsonObject.GetField("avatar", delegate(JSONObject avatar)
            {
                this.avatar = avatar.ToString();
            });
            jsonObject.GetField("city", delegate(JSONObject city)
            {
                this.city = city.ToString();
            });
            jsonObject.GetField("country", delegate(JSONObject country)
            {
                this.country = country.ToString();
            });
            jsonObject.GetField("dateOfBirth", delegate(JSONObject dateOfBirth)
            {
                this.dateOfBirth = dateOfBirth.ToString();
            });
            jsonObject.GetField("displayName", delegate(JSONObject displayName)
            {
                this.displayName = displayName.ToString();
            });
            jsonObject.GetField("encodedId", delegate(JSONObject encodedId)
            {
                this.encodedId = encodedId.ToString();
            });
            jsonObject.GetField("fullName", delegate(JSONObject fullName)
            {
                this.fullName = fullName.ToString();
            });
            jsonObject.GetField("gender", delegate(JSONObject gender)
            {
                this.gender = gender.ToString();
            });
            jsonObject.GetField("nickname", delegate(JSONObject nickname)
            {
                this.nickname = nickname.ToString();
            });
            jsonObject.GetField("timezone", delegate(JSONObject timezone)
            {
                this.timezone = timezone.ToString();
            });
            jsonObject.GetField("gender", delegate(JSONObject gender)
            {
                this.gender = gender.ToString();
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

        public string ToString()
        {
            string str = "aboutMe: " + aboutMe+ "\navatar: "+avatar+ "\ncity: "+ city+"\ncountry: "+ country+"\ndateOfBirth: "+dateOfBirth+
                "\ndisplayName: "+displayName+"\nencodedId: "+ encodedId+"\nfullName: "+fullName+"\ngneder: "+ gender+
                 "\nnickname: "+nickname+ "\nstate: "+state+"\ntimezone: "+timezone+"\nheight: "+height+
                 "\noffsetFromUTCMillis: "+offsetFromUTCMillis+"\nstrideLengthRunning: "+strideLengthRunning+"\nstrideLengthWalking: "+ 
                 strideLengthWalking+ "\nweight: "+weight;
            return str;
        }

    }
}
