﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KategoriTanim.aspx.cs" Inherits="serkanISG.KategoriTanim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css" integrity="sha384-WskhaSGFgHYWDcbwN70/dfYBj47jz9qbsMId/iRN3ewGhXQFZCSftd1LZCfmhktB" crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.3.1.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.1/js/bootstrap.min.js" integrity="sha384-smHYKdLADwkXOn1EmN1qk/HfnUcbVRZyYmZ4qpPea6sjB/pTJ0euyQp0Mk8ck+5T" crossorigin="anonymous"></script>
    <link href="../Content/MyCSS.css" rel="stylesheet" />
    <script src="../Scripts/MyFunctions.js"></script>


    <div class="pd-ltr-20 xs-pd-20-10" style="margin-left: 10%; width: 1230px;">
        <div class="min-height-200px">

            <!-- Simple Datatable start -->
            <div class="card-box mb-190">
                <div class="pd-20" style="margin-top: 100px">
                    <h4 class="text-blue h4 text-center">KATEGORİ TANIM LİSTESİ</h4>
                </div>
            </div>
            <br />
            <ul>
                <li style="float: left">
                    <button type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#myyModal">Yeni Kategori Tanımı Ekle</button>
                    <br />


                </li>

                <li style="float: left;">
                    <asp:DropDownList ID="ddlAktiflik" AutoPostBack="true" OnSelectedIndexChanged="ddlAktiflik_SelectedIndexChanged" CssClass="form-control" Style="width: 200px; margin-left: 350%" runat="server">

                        <asp:ListItem Selected hidden Text="Listeleme Durumu" />
                        <asp:ListItem Text="Aktif" />
                        <asp:ListItem Text="Pasif" />
                    </asp:DropDownList>
                </li>

            </ul>
            <br />
            <div style="width: 100%; height: 400px; overflow: scroll">
                <asp:GridView ID="grdKATEGORI" CssClass="data-table table stripe hover nowrap" AutoGenerateColumns="False" GridLines="None" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="Düzen">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" CssClass="btn btn-warning" ID="link" Text="<i class='icon-copy fi-pencil'></i>" ToolTip="Düzenle" CausesValidation="false" CommandName="Select" CommandArgument='<%# Eval("ID_KATEGORI") %>' OnClick="link_Click"></asp:LinkButton>
                                <div class="pasif">
                                    <asp:LinkButton runat="server" Style="margin-left: 55px; margin-top: -64px;" Text="<i class='fa fa-trash-o' aria-hidden='true'></i>" ToolTip="Sil" CssClass="btn btn-danger" ID="btn_sil" OnClientClick="return fnConfirmDelete();" CausesValidation="false" CommandArgument='<%# Eval("ID_KATEGORI") %>' OnClick="btn_sil_Click"></asp:LinkButton>
                                </div>
                                <div style="display: none;" class="reload">
                                    <asp:LinkButton runat="server" Style="margin-left: 49px; margin-top: -64px;" CssClass="btn btn-success" Text="<i class='fa fa-refresh' aria-hidden='true'></i>" ToolTip="Aktif Et" ID="reload" OnClick="reload_Click" OnClientClick="return fnConfirmActive();" CausesValidation="false" CommandArgument='<%# Eval("ID_KATEGORI") %>'></asp:LinkButton>

                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID_KATEGORI" HeaderText="KATEGORİ KODU" />
                        <asp:BoundField DataField="AD_KATEGORI" HeaderText="KATOGORİ ADI" />
                        <asp:BoundField DataField="DURUM" HeaderText="Aktiflik" />

                    </Columns>
                </asp:GridView>
            </div>
            <div id="myModal" class="modal" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Kategori Tanım Düzenleme Ekranı</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Label ID="Label2" runat="server" Text="Kategori Kodu:" />
                                    &nbsp; 
                            <asp:Label ID="lbl_KategoriEdit" runat="server" Text="Label" />
                                </div>
                            </div>

                            <hr />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Label ID="Label1" runat="server" Text="Kategori Adı:">
                                                                        
                                    </asp:Label><asp:TextBox CssClass="form-control" ID="txtKategoriAd_edit" runat="server"></asp:TextBox><br />
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="Label3" runat="server" Text="Durum" />
                                    <asp:DropDownList ID="ddlDurumEdit" CssClass=" form-control" runat="server">
                                        <asp:ListItem Text="Aktif" />
                                        <asp:ListItem Text="Pasif" />
                                    </asp:DropDownList>
                                </div>


                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Kapat</button>
                            <asp:Button Text="Güncelle" CausesValidation="false" OnClientClick="fnConfirmUpdate();" ID="edit_Kaydet" class="btn btn-success" OnClick="edit_Kaydet_Click" runat="server" />

                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>



    <!-- Button trigger modal -->


    <!-- Modal -->
    <div class="modal fade" id="myyModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Kategori Tanım Ekranı</h4>
                    <button type="button" id="kapa" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                </div>
                <div class="modal-body">
                    <label class="form-group-item">Kategori Adı*</label>
                    <asp:TextBox class="form-control" ClientId="clientISADI" ID="txtKategoriAdi" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ss" ControlToValidate="txtKategoriAdi" ForeColor="red" runat="server" ErrorMessage="Bu Alan Zorunludur"></asp:RequiredFieldValidator>


                </div>
                <div class="modal-footer">

                    <button type="button" id="btn_Vazgec" class="btn btn-secondary" data-dismiss="modal">Vazgeç</button>
                    <asp:Button ID="btnNewKategori" CssClass="btn btn-primary" OnClick="btnNewKategori_Click" margin-top="4px;" runat="server" Text="Ekle" />

                </div>
            </div>
        </div>
    </div>



    <div id="snackbar" style="width: 20%; overflow: visible; margin-left: 60%; position: relative; min-height: 40%;">Kayıt Eklendi</div>



    <script>
        $("#btn_Vazgec").click(function () {

            window.location.href = "https://localhost:44398/TANIM/KategoriTanim";

        })
        $("#kapa").click(function () {

            window.location.href = "https://localhost:44398/TANIM/KategoriTanim";

        })




        function fnConfirmDelete() {
            return confirm("İş Tanımını silmek üzeresiniz. Onaylıyor musunuz?");
        }

        //function fnConfirmUpdate() {
        //    return confirm("İş Tanımını güncellemek üzeresiniz. Onaylıyor musunuz?");
        //}

        if ($('#<%= ddlAktiflik.ClientID %>').val() == "Pasif") {
            $(".pasif").css("display", "none");
            $(".reload").css("display", "block");

        }
        function fnConfirmActive() {
            return confirm("Kategori tanımını aktife almak istiyor musunuz?");
        }


    </script>






</asp:Content>
