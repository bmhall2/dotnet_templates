DROP TABLE IF EXISTS public.user_group;
DROP TABLE IF EXISTS public."group";
DROP TABLE IF EXISTS public."user";

CREATE TABLE IF NOT EXISTS public."group"
(
    "Id" uuid NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT group_pkey PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."group"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."user"
(
    "Id" uuid NOT NULL,
    "Name" text COLLATE pg_catalog."default",
    CONSTRAINT user_pkey PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."user"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public.user_group
(
    "UserId" uuid NOT NULL,
    "GroupId" uuid NOT NULL,
    CONSTRAINT user_group_pkey PRIMARY KEY ("UserId", "GroupId"),
    CONSTRAINT "FK_Group_Id" FOREIGN KEY ("GroupId")
        REFERENCES public."group" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "FK_User_Id" FOREIGN KEY ("UserId")
        REFERENCES public."user" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.user_group
    OWNER to postgres;


INSERT INTO public."group"(
	"Id", "Name")
	VALUES ('b6617722-132c-40e3-b55c-bf9502ba0dd4', 'Admins');

INSERT INTO public."user"(
	"Id", "Name")
	VALUES ('d71eec0f-2e35-402c-9d12-88a3c704862d', 'Benjamin Hall');

INSERT INTO public."user"(
	"Id", "Name")
	VALUES ('6636651c-d618-4ffe-82c9-6c880e11f168', 'Samantha Hall');

INSERT INTO public.user_group(
	"UserId", "GroupId")
	VALUES ('d71eec0f-2e35-402c-9d12-88a3c704862d', 'b6617722-132c-40e3-b55c-bf9502ba0dd4');