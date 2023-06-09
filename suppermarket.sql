PGDMP     0                    {            SupperMarket    15.2    15.1     	           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            
           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    33734    SupperMarket    DATABASE     �   CREATE DATABASE "SupperMarket" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';
    DROP DATABASE "SupperMarket";
                postgres    false            �            1255    33786    decrease_count()    FUNCTION     (  CREATE FUNCTION public.decrease_count() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
	begin
		update products
		set amount = amount - (select amount from soldproducts where id = (select max(id) from soldproducts))
		where id = (select max(productid) from soldproducts);
		return new;
	end;
$$;
 '   DROP FUNCTION public.decrease_count();
       public          postgres    false            �            1255    33785    selectoverallstatsasync()    FUNCTION     y  CREATE FUNCTION public.selectoverallstatsasync() RETURNS TABLE(productid bigint, soldproductcount bigint, soldproductmoney numeric)
    LANGUAGE sql
    AS $$
		select 
			p.id as ProductId, 
			sum(sp.amount) as SoldProductCount,
			(sum(sp.amount) * p.price) as SoldProductMoney
		from SoldProducts as sp
		inner join Products as p on p.id = sp.productid
		group by p.id
$$;
 0   DROP FUNCTION public.selectoverallstatsasync();
       public          postgres    false            �            1259    33759    products    TABLE     �   CREATE TABLE public.products (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    price numeric NOT NULL,
    amount bigint,
    createdat timestamp without time zone DEFAULT now(),
    updatedat timestamp without time zone
);
    DROP TABLE public.products;
       public         heap    postgres    false            �            1259    33758    products_id_seq    SEQUENCE     �   CREATE SEQUENCE public.products_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.products_id_seq;
       public          postgres    false    215                       0    0    products_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.products_id_seq OWNED BY public.products.id;
          public          postgres    false    214            �            1259    41932    soldproducts    TABLE     �   CREATE TABLE public.soldproducts (
    id integer NOT NULL,
    productid bigint,
    amount bigint,
    totalprice numeric,
    createdat timestamp without time zone,
    updatedat timestamp without time zone
);
     DROP TABLE public.soldproducts;
       public         heap    postgres    false            �            1259    41931    soldproducts_id_seq    SEQUENCE     �   CREATE SEQUENCE public.soldproducts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.soldproducts_id_seq;
       public          postgres    false    217                       0    0    soldproducts_id_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public.soldproducts_id_seq OWNED BY public.soldproducts.id;
          public          postgres    false    216            l           2604    33762    products id    DEFAULT     j   ALTER TABLE ONLY public.products ALTER COLUMN id SET DEFAULT nextval('public.products_id_seq'::regclass);
 :   ALTER TABLE public.products ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    214    215    215            n           2604    41935    soldproducts id    DEFAULT     r   ALTER TABLE ONLY public.soldproducts ALTER COLUMN id SET DEFAULT nextval('public.soldproducts_id_seq'::regclass);
 >   ALTER TABLE public.soldproducts ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    216    217    217                      0    33759    products 
   TABLE DATA           Q   COPY public.products (id, name, price, amount, createdat, updatedat) FROM stdin;
    public          postgres    false    215   R                 0    41932    soldproducts 
   TABLE DATA           _   COPY public.soldproducts (id, productid, amount, totalprice, createdat, updatedat) FROM stdin;
    public          postgres    false    217   )                  0    0    products_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.products_id_seq', 9, true);
          public          postgres    false    214                       0    0    soldproducts_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.soldproducts_id_seq', 17, true);
          public          postgres    false    216            p           2606    33767    products products_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.products DROP CONSTRAINT products_pkey;
       public            postgres    false    215            r           2606    41939    soldproducts soldproducts_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.soldproducts
    ADD CONSTRAINT soldproducts_pkey PRIMARY KEY (id);
 H   ALTER TABLE ONLY public.soldproducts DROP CONSTRAINT soldproducts_pkey;
       public            postgres    false    217            t           2620    41945    soldproducts decreaser    TRIGGER     t   CREATE TRIGGER decreaser AFTER INSERT ON public.soldproducts FOR EACH ROW EXECUTE FUNCTION public.decrease_count();
 /   DROP TRIGGER decreaser ON public.soldproducts;
       public          postgres    false    219    217            s           2606    41940 (   soldproducts soldproducts_productid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.soldproducts
    ADD CONSTRAINT soldproducts_productid_fkey FOREIGN KEY (productid) REFERENCES public.products(id) ON DELETE CASCADE;
 R   ALTER TABLE ONLY public.soldproducts DROP CONSTRAINT soldproducts_productid_fkey;
       public          postgres    false    3184    217    215               �   x�u�=NA���s
:���o�<>��B�Ҍ`�D�dW
nώ��`ɕߧ0h��H3�lF�q�	X�D}	" �.��~���I���M�}���ޗ��!0���Fͥ�L/�<���6�M� c��(�n
������v:_(�آXT�8��*��k��e�M'ؔԚr'J���<�>3���)#�=%�GTqG����Mw         �   x�uй�0D�����ţW���0D��7�P����HE����e�B��o�9����M�h�~���X0r��U�t�i�)]�7uJRB��0����Ш&/TCr�Y�x9�l0dh�m;������/��Ͻ�����q>�     