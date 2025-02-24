// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Net;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SiteOfRefuge.API.Models;
using Newtonsoft.Json.Linq;

namespace SiteOfRefuge.API
{
    public class RefugeesApi
    {
        /// <summary> Initializes a new instance of RefugeesApi. </summary>
        public RefugeesApi() {}

        /// <summary> Get a summary list of refugees registered in the system. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        [Function(nameof(GetRefugees))]
        public HttpResponseData GetRefugees([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "refugees")] HttpRequestData req, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(GetRefugees));
            logger.LogInformation($"HTTP trigger function processed a request.");

            IEnumerable<RefugeeSummary> refugeeList = new List<RefugeeSummary> {
                new RefugeeSummary( Guid.NewGuid(), "PL-06", 1, new DateTimeOffset(DateTime.UtcNow) ),
                new RefugeeSummary( Guid.NewGuid(), "PL-16", 4, new DateTimeOffset(DateTime.UtcNow) )
            };

            var okResponse = req.CreateResponse(HttpStatusCode.OK);
            okResponse.WriteAsJsonAsync(refugeeList);
            
            return okResponse;
        }

        const string PARAM_REFUGEE_ID = "@Id";
        const string PARAM_REFUGEESUMMARY_ID = "@Id";
        const string PARAM_REFUGEESUMMARY_REGION = "@Region";
        const string PARAM_REFUGEESUMMARY_PEOPLE = "@People";
        const string PARAM_REFUGEESUMMARY_MESSAGE = "@Message";
        const string PARAM_REFUGEESUMMARY_POSSESSIONDATE = "@PossessionDate";
        const string PARAM_REFUGEE_SUMMARY = "@Summary";
        const string PARAM_REFUGEE_CONTACT = "@Contact";
        const string PARAM_REFUGEESUMMARYTORESTRICTIONS_REFUGEESUMMARYID = "@RefugeeSummaryId";
        const string PARAM_REFUGEESUMMARYTORESTRICTIONS_RESTRICTIONVALUE = "@RestrictionValue";
        const string PARAM_REFUGEESUMMARYTOLANGUAGES_REFUGEESUMMARYID = "@RefugeeSummaryId";
        const string PARAM_REFUGEESUMMARYTOLANGUAGES_LANGUAGEVALUE = "@LanguageValue";
        const string PARAM_REFUGEESUMMARYTOLANGUAGES_SUMMARYID = "@SummaryId";
        const string PARAM_REFUGEESUMMARYTORESTRICTIONS_SUMMARYID = "@SummaryId";        /// <summary> Registers a new refugee in the system. </summary>
        /// <summary> Registers a new refugee in the system. </summary>
        /// <param name="body"> The Refugee to use. </param>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="body"/> is null. </exception>
        [Function(nameof(AddRefugee))]
        public HttpResponseData AddRefugee([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "refugees")] Refugee body, HttpRequestData req, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(AddRefugee));
            logger.LogInformation("HTTP trigger function processed a request.");

            if(body == null)
                throw new ArgumentNullException();
            
            //WARNING: trusting Id in body.Id (is this passed in from the request?) -- if we need to use an auth thing will need some code to update this
            using(SqlConnection sql = SqlShared.GetSqlConnection())
            {
                sql.Open();
                using(SqlCommand cmd = new SqlCommand($"select top 1 * from Refugee where Id = {PARAM_REFUGEE_ID}" , sql))
                {
                    cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEE_ID, System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters[PARAM_REFUGEE_ID].Value = body.Id;
                    using(SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if(sdr.Read())
                        {
                            var resp = req.CreateResponse(HttpStatusCode.BadRequest);
                            resp.WriteStringAsync("Error: trying to create a refugee with Id '" + body.Id.ToString() + "' but a refugee with this Id already exists in the database.");
                            return resp;
                        }
                    }
                }

                using(SqlTransaction transaction = sql.BeginTransaction())
                {
                    try
                    {
                        SqlShared.InsertContactModes(sql, transaction, body.Contact.Methods);
                        SqlShared.InsertContact(sql, transaction, body.Contact);
                        SqlShared.InsertContactToMethods(sql, transaction, body.Contact.Methods, body.Contact.Id);

                        using(SqlCommand cmd = new SqlCommand($@"insert into RefugeeSummary(Id, Region, People, Message, PossessionDate) values(
                            {PARAM_REFUGEESUMMARY_ID}, {PARAM_REFUGEESUMMARY_REGION}, {PARAM_REFUGEESUMMARY_PEOPLE}, {PARAM_REFUGEESUMMARY_MESSAGE}, {PARAM_REFUGEESUMMARY_POSSESSIONDATE});", sql, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEESUMMARY_ID, System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters[PARAM_REFUGEESUMMARY_ID].Value = body.Summary.Id;
                            cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEESUMMARY_REGION, System.Data.SqlDbType.NVarChar));
                            cmd.Parameters[PARAM_REFUGEESUMMARY_REGION].Value = body.Summary.Region;
                            cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEESUMMARY_PEOPLE, System.Data.SqlDbType.Int));
                            cmd.Parameters[PARAM_REFUGEESUMMARY_PEOPLE].Value = body.Summary.People;
                            cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEESUMMARY_MESSAGE, System.Data.SqlDbType.NVarChar));
                            cmd.Parameters[PARAM_REFUGEESUMMARY_MESSAGE].Value = body.Summary.Message;
                            cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEESUMMARY_POSSESSIONDATE, System.Data.SqlDbType.DateTimeOffset));
                            cmd.Parameters[PARAM_REFUGEESUMMARY_POSSESSIONDATE].Value = body.Summary.PossessionDate;
                            cmd.ExecuteNonQuery();
                        }

                        const string TABLE_NAME = "Refugee";
                        SqlShared.InsertCustomer(sql, transaction, body.Id, body.Summary.Id, body.Contact.Id, TABLE_NAME);

                        const string RESTRICTIONS_TABLE_NAME = "RefugeeSummaryToRestrictions";
                        const string RESTRICTIONS_ID_COLUMN_NAME = "RefugeeSummaryId";
                        SqlShared.InsertRestrictionsList(sql, transaction, body.Summary.Restrictions, RESTRICTIONS_TABLE_NAME, RESTRICTIONS_ID_COLUMN_NAME, body.Summary.Id);

                        const string LANGUAGES_TABLE_NAME = "RefugeeSummaryToLanguages";
                        const string LANGUAGES_ID_COLUMN_NAME = "RefugeeSummaryId";
                        SqlShared.InsertLanguageList(sql, transaction, body.Summary.Languages, LANGUAGES_TABLE_NAME, LANGUAGES_ID_COLUMN_NAME, body.Summary.Id);
                    }
                    catch (Exception exc)
                    {
                        transaction.Rollback();
                        var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                        badResponse.WriteStringAsync(exc.ToString());
                        return badResponse;
                        //return new BadRequestResult();
                    }
                    transaction.Commit();
                }
                
                sql.Close();
            }
            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 201
            return req.CreateResponse(HttpStatusCode.Created);
        }

        /// <summary> Get information about a specific refugee. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="id"> Refugee id in UUID/GUID format. </param>
        [Function(nameof(GetRefugee))]
        public HttpResponseData GetRefugee([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "refugees/{id}")] HttpRequestData req, Guid id, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(GetRefugee));
            logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            try
            {
                using(SqlConnection sql = SqlShared.GetSqlConnection())
                {
                    sql.Open();

                    JObject json = new JObject();
                    Guid? contactId = null;
                    Guid? summaryId = null;

                    using(SqlCommand cmd = new SqlCommand($@"select r.id as Id,
                        rs.id as RefugeeSummaryId,
                        rs.Region as RefugeeSummaryRegion,
                        rs.People as RefugeeSummaryPeople,
                        rs.Message as RefugeeSummaryMessage,
                        rs.PossessionDate as RefugeePossessionDate,
                        c.Id as RefugeeContactId,
                        c.Name as RefugeeContactName
                        from refugee r
                        join refugeesummary rs on r.summary = rs.id
                        join contact c on r.contact = c.id
                        where r.Id = {PARAM_REFUGEE_ID}", sql))
                    {
                        cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEE_ID, System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters[PARAM_REFUGEE_ID].Value = id;
                        using(SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if(!sdr.Read())
                            {
                                var resp2 = req.CreateResponse(HttpStatusCode.BadRequest);
                                resp2.WriteStringAsync("Error: no refugee with ID '" + id.ToString() + "'");
                                return resp2;
                                //return new BadRequestObjectResult("Error: no refugee with ID '" + id.ToString() + "'");
                            }
                            json["id"] = sdr.GetGuid(0).ToString();

                            //summary portion
                            JObject summary = new JObject();
                            summaryId = sdr.GetGuid(1);
                            summary["id"] = summaryId.ToString();
                            summary["region"] = sdr.GetString(2);
                            summary["people"] = sdr.GetInt32(3);
                            summary["message"] = sdr.GetString(4);
                            //restrictions
                            //languages
                            summary["possession_date"] = sdr.GetDateTimeOffset(5);
                            json["summary"] = summary;

                            //contact portion
                            JObject contact = new JObject();
                            contactId = sdr.GetGuid(6);
                            contact["id"] = contactId.ToString();
                            contact["name"] = sdr.GetString(7);
                            json["contact"] = contact;

                        }
                    }

                    const string PARAM_CONTACTTOMETHODS_CONTACTID = "@ContactId";
                    using(SqlCommand cmd = new SqlCommand($@"select cm.Id,
                        cmm.description,
                        cm.Value,
                        cm.verified
                        from contacttomethods ctm
                        join contactmode cm on ctm.contactmodeid = cm.id
                        join contactmodemethod cmm on cm.method = cmm.id
                        where ctm.contactid = {PARAM_CONTACTTOMETHODS_CONTACTID}", sql))
                    {
                        cmd.Parameters.Add(new SqlParameter(PARAM_CONTACTTOMETHODS_CONTACTID, System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters[PARAM_CONTACTTOMETHODS_CONTACTID].Value = contactId;
                        List<JObject> contactMethods = new List<JObject>();
                        using(SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if(!sdr.Read())
                            {
                                var resp2 = req.CreateResponse(HttpStatusCode.BadRequest);
                                resp2.WriteStringAsync("Error: no contact with ID '" + contactId.ToString() + "'");
                                return resp2;
                                //return new BadRequestObjectResult("Error: no contact with ID '" + contactId.ToString() + "'");
                            }
                            JObject contactMethod = new JObject();
                            contactMethod["id"] = sdr.GetGuid(0).ToString();
                            contactMethod["method"] = sdr.GetString(1);
                            contactMethod["value"] = sdr.GetString(2);
                            bool verified = sdr.GetBoolean(3);
                            contactMethod["verified"] = verified;
                            contactMethods.Add(contactMethod);
                        }
                        json["contact"]["methods"] = JToken.FromObject(contactMethods);
                    }

                    using(SqlCommand cmd = new SqlCommand($@"select sl.description
                        from refugeesummarytolanguages rstl
                        join spokenlanguages sl on rstl.spokenlanguagesid = sl.id
                        where rstl.refugeesummaryid = {PARAM_REFUGEESUMMARYTOLANGUAGES_SUMMARYID}", sql))
                    {
                        cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEESUMMARYTOLANGUAGES_SUMMARYID, System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters[PARAM_REFUGEESUMMARYTOLANGUAGES_SUMMARYID].Value = summaryId;
                        List<string> languages = new List<string>();
                        using(SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            int found = 0;
                            while(sdr.Read())
                            {
                                found++;
                                languages.Add(sdr.GetString(0));
                            }
                            //QUESTION: it's okay not to restrict to a particular language, right? or need 1+?
                            //if(found < 1)
                            //    return new BadRequestObjectResult("Error: no contact with ID '" + contactId.ToString() + "'");
                        }
                        json["summary"]["languages"] = JToken.FromObject(languages);
                    }

                    using(SqlCommand cmd = new SqlCommand($@"select r.description
                        from refugeesummarytorestrictions rstr
                        join Restrictions r on rstr.restrictionsid = r.id
                        where rstr.refugeesummaryid = {PARAM_REFUGEESUMMARYTORESTRICTIONS_SUMMARYID}", sql))
                    {
                        cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEESUMMARYTORESTRICTIONS_SUMMARYID, System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters[PARAM_REFUGEESUMMARYTORESTRICTIONS_SUMMARYID].Value = summaryId;
                        List<string> restrictions = new List<string>();
                        using(SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            int found = 0;
                            while(sdr.Read())
                            {
                                found++;
                                restrictions.Add(sdr.GetString(0));
                            }
                            //QUESTION: it's okay not to restrict to a particular language, right? or need 1+?
                            //if(found < 1)
                            //    return new BadRequestObjectResult("Error: no contact with ID '" + contactId.ToString() + "'");
                        }
                        json["summary"]["restrictions"] = JToken.FromObject(restrictions);
                    }

                    sql.Close();

                    var resp = req.CreateResponse(HttpStatusCode.OK);
                    resp.WriteAsJsonAsync(json.ToString());
                    return resp;
                }
            }
            catch(Exception exc)
            {
                //return new BadRequestObjectResult(exc.ToString()); //TODO: DEBUG, not good for real site
                //return new StatusCodeResult(404);
                var badResponse = req.CreateResponse(HttpStatusCode.NotFound);
                badResponse.WriteStringAsync(exc.ToString());
                return badResponse;
            }
            // Spec Defines: HTTP 200
            // Spec Defines: HTTP 404
        }

        /// <summary> Updates a refugee in the system. </summary>
        /// <param name="id"> Refugee id in UUID/GUID format. </param>
        /// <param name="body"> The Refugee to use. </param>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="body"/> is null. </exception>
        [Function(nameof(UpdateRefugee))]
        public HttpResponseData UpdateRefugee(Guid id, [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "refugees/{id}")] Refugee body, HttpRequestData req, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(UpdateRefugee));
            logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 204
            // Spec Defines: HTTP 404

            throw new NotImplementedException();
        }

        /// <summary> Schedules a refugee to be deleted from the system (after 7 days archival). </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="id"> Refugee id in UUID/GUID format. </param>
        [Function(nameof(DeleteRefugee))]
        public HttpResponseData DeleteRefugee([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "refugees/{id}")] HttpRequestData req, Guid id, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(DeleteRefugee));
            logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            try
            {
                using(SqlConnection sql = SqlShared.GetSqlConnection())
                {
                    sql.Open();

                    using(SqlCommand cmd = new SqlCommand($@"
                        BEGIN TRANSACTION;
                        GO
                        declare @refugeeid uniqueidentifier;
                        set @refugeeid = {PARAM_REFUGEE_ID};

                        declare @contactid uniqueidentifier;
                        set @contactid = (select top 1 Contact from Refugee where Id = @refugeeid);
                        declare @summaryid uniqueidentifier;
                        set @summaryid = (select top 1 Summary from Refugee where Id = @refugeeid);

                        select contactmodeid into #contactmodestodelete from contacttomethods where contactid = @contactid;
                        delete from contacttomethods where contactid = @contactid;
                        delete from contactmode where id in (select contactmodeid from #contactmodestodelete);
                        drop table #contactmodestodelete;

                        delete from refugeesummarytolanguages where refugeesummaryid = @summaryid;
                        delete from refugeesummarytorestrictions where refugeesummaryid = @summaryid;

                        delete from refugee where id = @refugeeid;
                        delete from refugeesummary where id = @summaryid;
                        delete from contact where id = @contactid;

                        GO
                        if @@error != 0 raiserror('Error deleting', 20, -1) with log
                        GO

                        COMMIT TRANSACTION;", sql))
                    {
                        cmd.Parameters.Add(new SqlParameter(PARAM_REFUGEE_ID, System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters[PARAM_REFUGEE_ID].Value = id;
                        cmd.ExecuteNonQuery();
                        return req.CreateResponse(HttpStatusCode.Accepted);
                    }
                }
            }
            catch(Exception exc)
            {
                //return new BadRequestObjectResult(exc.ToString()); //TODO: DEBUG, not good for real site
                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            // Spec Defines: HTTP 202
            // Spec Defines: HTTP 404
        }
    }
}
