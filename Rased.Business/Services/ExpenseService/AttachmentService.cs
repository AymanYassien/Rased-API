using System.Linq.Expressions;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Business.Services.ExpenseService;

public class AttachmentService : IAttachmentService
{
    private IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;
    private ApiResponse<object> _response;
    
    public AttachmentService(IUnitOfWork unitOfWork , IWebHostEnvironment env)
    {
        _unitOfWork = unitOfWork;
        _env = env;
        _response = new ApiResponse<object>();
    }
    
    public async Task<ApiResponse<object>> GetAttachmentById(int id)
    {
        if (1 > id)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  await _unitOfWork.Attachments.GetByIdAsync(id);
        if (res is null)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);

        var mapped = MapAttachmentToAttachmentDTO(res);
        
        return _response.Response(true, mapped, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> GetAttachmentByDraftId(int draftId, Expression<Func<Infrastructure.Attachment, bool>>[]? filter = null)
    {
        if (1 > draftId)
            return _response.Response(false, null, "",
                "Bad Request", HttpStatusCode.BadRequest);

        // «·»ÕÀ ⁄‰ «·‹ Attachments »«” Œœ«„ «·‹ DraftId
        var res = await _unitOfWork.Attachments.GetAttachmentByDraftId(draftId, filter);
        if (res is null)
            return _response.Response(false, null, "", "Not Found, or fail to delete", HttpStatusCode.NotFound);

        //  ÕÊÌ· «·»Ì«‰«  „‰ Attachment ≈·Ï DTO
        var mapped = MapAttachmentToAttachmentDTO(res);

        return _response.Response(true, mapped, "Success", "", HttpStatusCode.OK);
    }



    public async Task<ApiResponse<object>> GetAttachmentByExpenseId(int expenseId, Expression<Func<Infrastructure.Attachment, bool>>[]? filter = null)
    {
        if (1 > expenseId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  await _unitOfWork.Attachments.GetAttachmentByExpenseId(expenseId, filter);
        if (res is null)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);

        var mapped = MapAttachmentToAttachmentDTO(res);
        
        return _response.Response(true, mapped, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> AddAttachment(AddAttachmentDto newAttachment)
    {
        if (!IsAttachmentDtoValid(newAttachment, out var errorMessage))
        {
            return _response.Response(false, newAttachment, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }

        var attach = addAttachment(newAttachment);

        try
        {
            await _unitOfWork.Attachments.AddAsync(attach);
            await _unitOfWork.CommitChangesAsync();
            
            return _response.Response(true, attach, $"Success add Attachment with id: {attach.AttachmentId}", $"",
                HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, newAttachment, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        
    }

    public async Task<ApiResponse<object>> UpdateAttachment(int attachmentId, UpdateAttachmentDto updateAttachment)
    {
        if (!IsAttachmentDtoValid(updateAttachment, out var errorMessage))
        {
            return _response.Response(false, updateAttachment, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        if (1 > attachmentId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.Attachments.GetByIdAsync(attachmentId);
        
        if (res == null)
            return _response.Response(false, null, "", $"Not Found Attachment with id {attachmentId}",  HttpStatusCode.NotFound);

        
        UpdateAttachment(updateAttachment, res);

        try
        {
            _unitOfWork.Attachments.Update(res);
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, updateAttachment, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        return _response.Response(true, res, $"Success Update Attachment with id: {res.AttachmentId}", $"",
            HttpStatusCode.NoContent);
    }

    public async Task<ApiResponse<object>> DeleteAttachment(int attachmentId)
    {
        if (1 > attachmentId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.Attachments.RemoveById(attachmentId);
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);


    }

    public async Task<ApiResponse<object>> GetFileSize(int attachmentId)
    {
        if (1 > attachmentId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);

        var res = await _unitOfWork.Attachments.GetFileSize(attachmentId);
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        return  _response.Response(true, res, "Success", "",  HttpStatusCode.OK);throw new NotImplementedException();

    }

    public async Task<ApiResponse<object>> GetFilePath(int attachmentId)
    {
        if (1 > attachmentId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);

        var res = await _unitOfWork.Attachments.GetFilePath(attachmentId);
        if (res is null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        return  _response.Response(true, res, "Success", "",  HttpStatusCode.OK);throw new NotImplementedException();

    }

    public async Task<ApiResponse<object>> GetFileType(int attachmentId)
    {
        if (1 > attachmentId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);

        var res = await _unitOfWork.Attachments.GetFileType(attachmentId);
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        return  _response.Response(true, res, "Success", "",  HttpStatusCode.OK);throw new NotImplementedException();
    }

    public async Task<ApiResponse<object>> GetFileUploadDate(int attachmentId)
    {
        if (1 > attachmentId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);

        var res = await _unitOfWork.Attachments.GetUploadDate(attachmentId);
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        return  _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }
    
    
    // Helpers
    
    private AttachmentDto MapAttachmentToAttachmentDTO(Attachment attachment)
    {
        return new AttachmentDto()
        {
            AttachmentId = attachment.AttachmentId,
            ExpenseId = attachment.ExpenseId,
            FileName = attachment.FileName,
            FilePath = attachment.FilePath,
            FileSize = attachment.FileSize,
            UploadDate = attachment.UploadDate,
            FileType = attachment.FileType
        };
    }

    private Attachment MapAddToAttachment(AddAttachmentDto dto)
    {
        return new Attachment()
        {
            ExpenseId = dto.ExpenseId,
            FileName = dto.FileName,
            FilePath = dto.FilePath,
            FileSize = dto.FileSize,
            FileType = dto.FileType
        };
    }
    
    private Attachment MapUpdateToAttachment(UpdateAttachmentDto dto, Attachment attachment)
    {
        attachment.AttachmentId = dto.AttachmentId;
        attachment.ExpenseId = dto.ExpenseId;
        attachment.FileName = dto.FileName;
        attachment.FilePath = dto.FilePath;
        attachment.FileSize = dto.FileSize;
        attachment.FileType = dto.FileType;

        return attachment;

    }

    private void UpdateAttachment(UpdateAttachmentDto dto, Attachment attachment)
    {
        attachment = MapUpdateToAttachment(dto, attachment);
        
        if (dto.FilePath != attachment.FilePath)
            attachment.UploadDate = DateTime.UtcNow;
        
        attachment.FilePath = dto.FilePath;
        attachment.FileName = dto.FileName;
        attachment.FileSize = dto.FileSize;
        
        //return attachment;
    }

    private Attachment addAttachment(AddAttachmentDto dto)
    {
        Attachment attach = MapAddToAttachment(dto);
        
        attach.UploadDate = DateTime.UtcNow;

        return attach;
    }

    private bool IsAttachmentDtoValid(AddAttachmentDto dto, out string errorMessage)
    {
        errorMessage = string.Empty;

        // 1. Title: Required, MaxLength(50)
        if (string.IsNullOrEmpty(dto.FilePath) ||
            string.IsNullOrEmpty(dto.FileName) ||
            string.IsNullOrEmpty(dto.FileType))
        {
            errorMessage = "File Path, Name, Type is required.";
            return false;
        }
        // 512 - 50 - 10 

        //if (dto.FilePath.Length > 512)
        //{
        //    errorMessage = "File Path Must < 512 Chars";
        //    return false;
        //}
        
        //if (dto.FileName.Length > 50)
        //{
        //    errorMessage = "File Name Must < 50 Chars";
        //    return false;
        //}
        
        //if (dto.FileType.Length > 10)
        //{
        //    errorMessage = "File Type Must < 10 Chars";
        //    return false;
        //}

        //if (dto.FileSize > long.MaxValue
        //    || dto.FileSize < 0 )
        //{
        //    errorMessage = $"File Size  Must < {long.MaxValue} And Must Positive Value";
        //    return false;
        //}
        
        return true;
        

    }

    private bool IsAttachmentDtoValid(UpdateAttachmentDto dto, out string errorMessage)
    {
        errorMessage = string.Empty;

        // 1. Title: Required, MaxLength(50)
        if (string.IsNullOrEmpty(dto.FilePath) ||
            string.IsNullOrEmpty(dto.FileName) ||
            string.IsNullOrEmpty(dto.FileType))
        {
            errorMessage = "File Path, Name, Type is required.";
            return false;
        }
        // 512 - 50 - 10 

        if (dto.FilePath.Length > 512)
        {
            errorMessage = "File Path Must < 512 Chars";
            return false;
        }
        
        if (dto.FileName.Length > 50)
        {
            errorMessage = "File Name Must < 50 Chars";
            return false;
        }
        
        if (dto.FileType.Length > 10)
        {
            errorMessage = "File Type Must < 10 Chars";
            return false;
        }

        if (dto.FileSize > long.MaxValue
            || dto.FileSize < 0 )
        {
            errorMessage = $"File Size  Must < {long.MaxValue} And Must Positive Value";
            return false;
        }
        
        return true;
        

    }





}