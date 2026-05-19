ALTER TABLE "Users"
ADD COLUMN IF NOT EXISTS "IsBlocked" boolean NOT NULL DEFAULT false;

INSERT INTO "Permissions" ("Id", "Name")
VALUES
    (121, 'UsersView'),
    (122, 'UsersAdd'),
    (123, 'UsersEdit'),
    (124, 'UsersDelete'),
    (125, 'UsersBlock')
ON CONFLICT ("Id") DO NOTHING;
