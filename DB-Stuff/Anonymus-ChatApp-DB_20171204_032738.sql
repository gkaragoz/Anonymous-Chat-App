--
-- PostgreSQL database dump
--

-- Dumped from database version 9.3.20
-- Dumped by pg_dump version 9.6.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: Account; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "Account" (
    "ID" integer NOT NULL,
    "LimitID" integer NOT NULL,
    "PricesID" integer NOT NULL,
    "AccountExpirationID" integer NOT NULL,
    "AccountType" integer NOT NULL
);


ALTER TABLE "Account" OWNER TO postgres;

--
-- Name: AccountExpiration; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "AccountExpiration" (
    "ID" integer NOT NULL,
    "StartDate" date NOT NULL,
    "EndDate" date NOT NULL
);


ALTER TABLE "AccountExpiration" OWNER TO postgres;

--
-- Name: AccountExpiration_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "AccountExpiration_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "AccountExpiration_ID_seq" OWNER TO postgres;

--
-- Name: AccountExpiration_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "AccountExpiration_ID_seq" OWNED BY "AccountExpiration"."ID";


--
-- Name: Account_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Account_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Account_ID_seq" OWNER TO postgres;

--
-- Name: Account_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Account_ID_seq" OWNED BY "Account"."ID";


--
-- Name: Limits; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "Limits" (
    "ID" integer NOT NULL,
    "TalksLimit" integer NOT NULL,
    "SpamLimit" integer NOT NULL
);


ALTER TABLE "Limits" OWNER TO postgres;

--
-- Name: Limits_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Limits_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Limits_ID_seq" OWNER TO postgres;

--
-- Name: Limits_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Limits_ID_seq" OWNED BY "Limits"."ID";


--
-- Name: Message; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "Message" (
    "ID" integer NOT NULL,
    "Body" character varying(2044) NOT NULL,
    "SendTime" timestamp without time zone NOT NULL,
    "TalksID" integer NOT NULL,
    "SenderID" integer NOT NULL
);


ALTER TABLE "Message" OWNER TO postgres;

--
-- Name: Message_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Message_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Message_ID_seq" OWNER TO postgres;

--
-- Name: Message_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Message_ID_seq" OWNED BY "Message"."ID";


--
-- Name: Prices; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "Prices" (
    "ID" integer NOT NULL,
    "Daily" double precision NOT NULL,
    "Weekly" double precision NOT NULL,
    "Monthly" double precision NOT NULL,
    "ThreeMonthly" double precision NOT NULL
);


ALTER TABLE "Prices" OWNER TO postgres;

--
-- Name: Prices_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Prices_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Prices_ID_seq" OWNER TO postgres;

--
-- Name: Prices_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Prices_ID_seq" OWNED BY "Prices"."ID";


--
-- Name: Talks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "Talks" (
    "ID" integer NOT NULL,
    "ReceiverID" integer NOT NULL,
    "SenderID" integer NOT NULL,
    "CreateDate" date NOT NULL
);


ALTER TABLE "Talks" OWNER TO postgres;

--
-- Name: Talks_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Talks_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Talks_ID_seq" OWNER TO postgres;

--
-- Name: Talks_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Talks_ID_seq" OWNED BY "Talks"."ID";


--
-- Name: User; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "User" (
    "ID" integer NOT NULL,
    "Nickname" character varying(2044) NOT NULL,
    "AvatarURL" character varying(2044) NOT NULL,
    "Language" character varying(2044) NOT NULL,
    "CreatedDate" date NOT NULL,
    "AccountID" integer NOT NULL
);


ALTER TABLE "User" OWNER TO postgres;

--
-- Name: User_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "User_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "User_ID_seq" OWNER TO postgres;

--
-- Name: User_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "User_ID_seq" OWNED BY "User"."ID";


--
-- Name: Account ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Account" ALTER COLUMN "ID" SET DEFAULT nextval('"Account_ID_seq"'::regclass);


--
-- Name: AccountExpiration ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "AccountExpiration" ALTER COLUMN "ID" SET DEFAULT nextval('"AccountExpiration_ID_seq"'::regclass);


--
-- Name: Limits ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Limits" ALTER COLUMN "ID" SET DEFAULT nextval('"Limits_ID_seq"'::regclass);


--
-- Name: Message ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Message" ALTER COLUMN "ID" SET DEFAULT nextval('"Message_ID_seq"'::regclass);


--
-- Name: Prices ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Prices" ALTER COLUMN "ID" SET DEFAULT nextval('"Prices_ID_seq"'::regclass);


--
-- Name: Talks ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Talks" ALTER COLUMN "ID" SET DEFAULT nextval('"Talks_ID_seq"'::regclass);


--
-- Name: User ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "User" ALTER COLUMN "ID" SET DEFAULT nextval('"User_ID_seq"'::regclass);


--
-- Data for Name: Account; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: AccountExpiration; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Name: AccountExpiration_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"AccountExpiration_ID_seq"', 1, false);


--
-- Name: Account_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"Account_ID_seq"', 1, false);


--
-- Data for Name: Limits; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Name: Limits_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"Limits_ID_seq"', 1, false);


--
-- Data for Name: Message; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Name: Message_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"Message_ID_seq"', 1, false);


--
-- Data for Name: Prices; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Name: Prices_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"Prices_ID_seq"', 1, false);


--
-- Data for Name: Talks; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Name: Talks_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"Talks_ID_seq"', 1, false);


--
-- Data for Name: User; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Name: User_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"User_ID_seq"', 1, false);


--
-- Name: AccountExpiration AccountExpiration_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "AccountExpiration"
    ADD CONSTRAINT "AccountExpiration_pkey" PRIMARY KEY ("ID");


--
-- Name: Account Account_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Account"
    ADD CONSTRAINT "Account_pkey" PRIMARY KEY ("ID");


--
-- Name: Limits Limits_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Limits"
    ADD CONSTRAINT "Limits_pkey" PRIMARY KEY ("ID");


--
-- Name: Message Message_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Message"
    ADD CONSTRAINT "Message_pkey" PRIMARY KEY ("ID");


--
-- Name: Prices Prices_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Prices"
    ADD CONSTRAINT "Prices_pkey" PRIMARY KEY ("ID");


--
-- Name: Talks Talks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Talks"
    ADD CONSTRAINT "Talks_pkey" PRIMARY KEY ("ID");


--
-- Name: User User_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("ID");


--
-- Name: Account lnk_AccountExpiration_Account; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Account"
    ADD CONSTRAINT "lnk_AccountExpiration_Account" FOREIGN KEY ("AccountExpirationID") REFERENCES "AccountExpiration"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: User lnk_Account_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "User"
    ADD CONSTRAINT "lnk_Account_User" FOREIGN KEY ("AccountID") REFERENCES "Account"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: Account lnk_Limits_Account; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Account"
    ADD CONSTRAINT "lnk_Limits_Account" FOREIGN KEY ("LimitID") REFERENCES "Limits"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: Account lnk_Prices_Account; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Account"
    ADD CONSTRAINT "lnk_Prices_Account" FOREIGN KEY ("PricesID") REFERENCES "Prices"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: Message lnk_Talks_Message; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Message"
    ADD CONSTRAINT "lnk_Talks_Message" FOREIGN KEY ("TalksID") REFERENCES "Talks"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: Talks lnk_User_Talks_Receiver; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Talks"
    ADD CONSTRAINT "lnk_User_Talks_Receiver" FOREIGN KEY ("ReceiverID") REFERENCES "User"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: Talks lnk_User_Talks_Sender; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Talks"
    ADD CONSTRAINT "lnk_User_Talks_Sender" FOREIGN KEY ("SenderID") REFERENCES "User"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

